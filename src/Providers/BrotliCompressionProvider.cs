﻿using Microsoft.AspNetCore.ResponseCompression;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AdminInmuebles.Providers
{
    public class BrotliCompressionProvider : ICompressionProvider
    {
        public string EncodingName => "br";
        public bool SupportsFlush => true;
        public Stream CreateStream(Stream outputStream) => new BrotliStream(outputStream, CompressionMode.Compress);

    }
}
