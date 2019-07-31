﻿//-----------------------------------------------------------------------------
// FILE:	    InternalResetPointInfo.cs
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

using Newtonsoft.Json;

using Neon.Cadence;
using Neon.Common;

namespace Neon.Cadence.Internal
{
    /// <summary>
    /// <b>INTERNAL USE ONLY:</b> Not sure what is.
    /// </summary>
    internal class InternalResetPointInfo
    {
        /// <summary>
        /// Not sure what is.
        /// </summary>
        [JsonProperty(PropertyName = "BinaryChecksum", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(null)]
        public string BinaryChecksum { get; set; }

        /// <summary>
        /// Not sure what is.
        /// </summary>
        [JsonProperty(PropertyName = "RunId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(null)]
        public string RunId { get; set; }

        /// <summary>
        /// Not sure what is.
        /// </summary>
        [JsonProperty(PropertyName = "FirstDecisionCompletedId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public long FirstDecisionCompletedId { get; set; }

        /// <summary>
        /// Not sure what is.
        /// </summary>
        [JsonProperty(PropertyName = "CreatedTimeNano", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public long CreatedTimeNano { get; set; }

        /// <summary>
        /// Not sure what is.
        /// </summary>
        [JsonProperty(PropertyName = "ExpiringTimeNano", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public long ExpiringTimeNano { get; set; }

        /// <summary>
        /// Not sure what is.
        /// </summary>
        [JsonProperty(PropertyName = "Resettable", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool Resettable { get; set; }

        /// <summary>
        /// Converts the instance into a public <see cref="WorkflowResetPoint"/>.
        /// </summary>
        public WorkflowResetPoint ToPublic()
        {
            return new WorkflowResetPoint()
            {
                BinaryChecksum           = this.BinaryChecksum,
                RunId                    = this.RunId,
                FirstDecisionCompletedId = this.FirstDecisionCompletedId,
                CreatedTime              = new DateTime(this.CreatedTimeNano / 100),
                ExpiringTime             = new DateTime(this.ExpiringTimeNano / 100),
                Resettable               = this.Resettable
            };
        }
    }
}
