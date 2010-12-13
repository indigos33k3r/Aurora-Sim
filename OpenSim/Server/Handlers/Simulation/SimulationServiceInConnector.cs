/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using Nini.Config;
using Aurora.Simulation.Base;
using OpenSim.Services.Interfaces;
using OpenSim.Framework;
using OpenSim.Framework.Servers.HttpServer;

namespace OpenSim.Server.Handlers.Simulation
{
    public class SimulationServiceInConnector : IService
    {
        private ISimulationService m_LocalSimulationService;
        //private IAuthenticationService m_AuthenticationService;
        public string Name
        {
            get { return GetType().Name; }
        }

        public void Initialize(IConfigSource config, IRegistryCore registry)
        {
        }

        public void PostInitialize(IConfigSource config, IRegistryCore registry)
        {
        }

        public void Start(IConfigSource config, IRegistryCore registry)
        {
        }

        public void PostStart(IConfigSource config, IRegistryCore registry)
        {
            IConfig handlerConfig = config.Configs["Handlers"];
            if (handlerConfig.GetString("SimulationInHandler", "") != Name)
                return;
            IHttpServer server = registry.Get<ISimulationBase>().GetHttpServer((uint)handlerConfig.GetInt("SimulationInHandlerPort"));

            //IConfig serverConfig = config.Configs["SimulationService"];
            //if (serverConfig == null)
            //    throw new Exception("No section 'SimulationService' in config file");

            //string simService = serverConfig.GetString("LocalServiceModule",
            //        String.Empty);

            //if (simService == String.Empty)
            //    throw new Exception("No SimulationService in config file");

            //Object[] args = new Object[] { config };
            m_LocalSimulationService = registry.Get<ISimulationService>();
            if(m_LocalSimulationService.GetInnerService() != null)
                m_LocalSimulationService = m_LocalSimulationService.GetInnerService();
            //ServerUtils.LoadPlugin<ISimulationService>(simService, args);

            //System.Console.WriteLine("XXXXXXXXXXXXXXXXXXX m_AssetSetvice == null? " + ((m_AssetService == null) ? "yes" : "no"));
            //server.AddStreamHandler(new AgentGetHandler(m_SimulationService, m_AuthenticationService));
            //server.AddStreamHandler(new AgentPostHandler(m_SimulationService, m_AuthenticationService));
            //server.AddStreamHandler(new AgentPutHandler(m_SimulationService, m_AuthenticationService));
            //server.AddStreamHandler(new AgentDeleteHandler(m_SimulationService, m_AuthenticationService));
            server.AddHTTPHandler("/agent/", new AgentHandler(m_LocalSimulationService).Handler);
            server.AddHTTPHandler("/object/", new ObjectHandler(m_LocalSimulationService).Handler);

            //server.AddStreamHandler(new ObjectPostHandler(m_SimulationService, authentication));
        }

        public void AddNewRegistry(IConfigSource config, IRegistryCore registry)
        {
        }
    }
}
