﻿//-----------------------------------------------------------------------------
// FILE:	    QueryHandlerAttribute.cs
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
using System.Diagnostics.Contracts;

using Neon.Cadence;
using Neon.Cadence.Internal;
using Neon.Common;

namespace Neon.Cadence
{
    /// <summary>
    /// Used to tag a <see cref="WorkflowBase"/> method that will be called to handle an
    /// external query.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryHandlerAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="queryName">Specifies the Cadence query name.</param>
        public QueryHandlerAttribute(string queryName)
        {
            Covenant.Requires<ArgumentNullException>(!string.IsNullOrEmpty(queryName));

            this.Name = queryName;
        }

        /// <summary>
        /// Returns the query name. 
        /// </summary>
        public string Name { get; private set; }
    }
}
