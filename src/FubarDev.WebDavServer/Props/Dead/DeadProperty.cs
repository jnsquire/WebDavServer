﻿// <copyright file="DeadProperty.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using FubarDev.WebDavServer.FileSystem;
using FubarDev.WebDavServer.Props.Store;

namespace FubarDev.WebDavServer.Props.Dead
{
    public class DeadProperty : IUntypedWriteableProperty, IDeadProperty
    {
        private readonly IPropertyStore _store;

        private readonly IEntry _entry;

        private XElement _cachedValue;

        public DeadProperty(IPropertyStore store, IEntry entry, XName name)
        {
            Name = name;
            _store = store;
            _entry = entry;
        }

        public DeadProperty(IPropertyStore store, IEntry entry, XElement element)
        {
            _store = store;
            _entry = entry;
            Name = element.Name;
        }

        public XName Name { get; }

        public IReadOnlyCollection<XName> AlternativeNames { get; } = new XName[0];

        public int Cost => _store.Cost;

        public Task SetXmlValueAsync(XElement element, CancellationToken ct)
        {
            _cachedValue = element;
            return _store.SetAsync(_entry, element, ct);
        }

        public async Task<XElement> GetXmlValueAsync(CancellationToken ct)
        {
            var result = _cachedValue ?? (_cachedValue = await _store.GetAsync(_entry, Name, ct).ConfigureAwait(false));
            if (result == null)
                throw new InvalidOperationException("Cannot get value from uninitialized property");
            return result;
        }

        public void Init(XElement initialValue)
        {
            _cachedValue = initialValue;
        }
    }
}
