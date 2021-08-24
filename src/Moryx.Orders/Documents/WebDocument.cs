// Copyright (c) 2021, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.IO;
using System.Net;
using System.Runtime.Serialization;

namespace Moryx.Orders.Documents
{
    /// <summary>
    /// Document which the file source is the web
    /// </summary>
    [DataContract]
    public class WebDocument : Document
    {
        /// <summary>
        /// Url to download the file
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebDocument()
        {
        }

        /// <summary>
        /// Constructor to create a web document
        /// </summary>
        public WebDocument(string number, short revision, string url) : base(number, revision)
        {
            Url = url;
        }

        /// <inheritdoc />
        public override Stream GetStream()
        {
            var request = WebRequest.Create(Url);
            var response = request.GetResponse();

            ContentType = response.ContentType;
            var dataStream = response.GetResponseStream();

            return dataStream;
        }
    }
}