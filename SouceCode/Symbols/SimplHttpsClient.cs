/*
 * Copyright (c) Troy Garner 2019 - Present
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 *
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using HttpsUtility.Https;
using HttpsUtility.Threading;

// ReSharper disable UnusedMember.Global

namespace HttpsUtility.Symbols
{
    /* --------------------------------------------  GENERIC SIMPL+ TYPE HELPER ALIASES  -------------------------------------------- */
    using STRING = String;             // string = STRING
    using SSTRING = SimplSharpString;  // SimplSharpString = STRING (used to interface with SIMPL+)
    using INTEGER = UInt16;            // ushort = INTEGER (unsigned)
    using SIGNED_INTEGER = Int16;      // short = SIGNED_INTEGER
    using SIGNED_LONG_INTEGER = Int32; // int = SIGNED_LONG_INTEGER
    using LONG_INTEGER = UInt32;
    using Newtonsoft.Json;
    using HttpsUtility.HeWeather;       // uint = LONG_INTEGER (unsigned)
    /* ------------------------------------------------------------------------------------------------------------------------------ */

    public sealed partial class SimplHttpsClient
    {
        private readonly Lazy<string> _moduleIdentifier;
        private readonly HttpsClient _httpsClient = new HttpsClient();
        private readonly SyncSection _httpsOperationLock = new SyncSection();
        
        public SimplHttpsClient()
        {
            _moduleIdentifier = new Lazy<string>(() =>
            {
                var asm = Assembly.GetExecutingAssembly().GetName();
                return string.Format("{0} {1}", asm.Name, asm.Version.ToString(2));
            });
        }
        
        private static IEnumerable<KeyValuePair<string, string>> ParseHeaders(STRING input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            
            var headerTokens = input.Split('|');
            return (from header in headerTokens
                let n = header.IndexOf(':')
                where n != -1
                select new KeyValuePair<string, string>(
                    header.Substring(0, n).Trim(),
                    header.Substring(n + 1).Trim())
                ).ToList();
        }

        private STRING MakeRequest(Func<HttpsResult> action)
        {
            using (_httpsOperationLock.AquireLock())
            {
                var response = action.Invoke();
                foreach (var contentChunk in response.Content.SplitIntoChunks(250))
                {
                    OnSimplHttpsClientResponse(response.Status, response.ResponseUrl, contentChunk, response.Content.Length);
                    CrestronEnvironment.Sleep(30); // allow for things to process
                }
                return (STRING)response.Content;
            }
        }

        public  STRING SendGet(STRING url, STRING headers)
        {
            return MakeRequest(() => _httpsClient.Get(url, ParseHeaders(headers)));
        }

        public STRING SendPost(STRING url, STRING headers, STRING content)
        {
            return MakeRequest(() => _httpsClient.Post(url, ParseHeaders(headers), content.NullIfEmpty()));
        }

        public STRING SendPut(STRING url, STRING headers, STRING content)
        {
            return MakeRequest(() => _httpsClient.Put(url, ParseHeaders(headers), content.NullIfEmpty()));
        }

        public STRING SendDelete(STRING url, STRING headers, STRING content)
        {
            return MakeRequest(() => _httpsClient.Delete(url, ParseHeaders(headers), content.NullIfEmpty()));
        }

        public override string ToString()
        {
            return _moduleIdentifier.Value;
        }

        // ------string to html code
        private  string GetHtmlEntities(string str)
        {
            string r = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                r += "&#" + Char.GetUnicodeCategory(str, i) + ";";
            }
            return r;
        }

        
        // ------HeWeather

        // getFeelTemperature
        public  int getFeelTemperature(string messageJson)
        {
            int FeelTemerature;
            var message = JsonConvert.DeserializeObject<Root>(messageJson);
            FeelTemerature = int.Parse(message.HeWeather6[0].now.fl);
            return FeelTemerature;
        }
        // getHumidity
        public  int getHumidity(string messageJson)
        {
            int humidity;
            var message = JsonConvert.DeserializeObject<Root>(messageJson);
            humidity = int.Parse(message.HeWeather6[0].now.hum);
            return humidity;
        }
        // getCond_txt
        public  string getCond_txt(string messageJson)
        {
            string Cond_txt;
            var message = JsonConvert.DeserializeObject<Root>(messageJson);
            Cond_txt = message.HeWeather6[0].now.cond_txt;
             
            // Cond_txt = GetHtmlEntities(Cond_txt);
            return Cond_txt;
        }
        // getCond_code
        public  int getCond_code(string messageJson)
        {
            int Cond_code;
            var message = JsonConvert.DeserializeObject<Root>(messageJson);
            Cond_code = int.Parse(message.HeWeather6[0].now.cond_code);
            return Cond_code;
        }
    }
}