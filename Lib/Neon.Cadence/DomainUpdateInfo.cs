﻿//-----------------------------------------------------------------------------
// FILE:	    DomainUpdateInfo.cs
// CONTRIBUTOR: Jeff Lill
// COPYRIGHT:	Copyright (c) 2016-2019 by neonFORGE, LLC.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Neon.Cadence;
using Neon.Cadence.Internal;
using Neon.Common;

namespace Neon.Cadence
{
    /// <summary>
    /// Holds the changes to be made to a Cadence domain's basic properties.
    /// </summary>
    public class DomainUpdateInfo
    {
        /// <summary>
        /// The updated domain description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The updated domain owner's email address.
        /// </summary>
        public string OwnerEmail { get; set; }
    }
}
