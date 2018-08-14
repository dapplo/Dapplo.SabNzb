//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.SabNzb
// 
//  Dapplo.SabNzb is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.SabNzb is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.SabNzb. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.SabNzb.Client.ViewModels;

namespace Dapplo.SabNzb.Client
{
    /// <inheritdoc />
    public class SabNzbClientModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<SabNzbTrayIconViewModel>()
                .As<ITrayIconViewModel>()
                .SingleInstance();

            builder.RegisterType<MainScreenViewModel>()
                .As<IShell>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<ConnectionViewModel>()
                .AsSelf()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
