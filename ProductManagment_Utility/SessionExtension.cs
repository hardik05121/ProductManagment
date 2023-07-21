﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_Utility

{ 

{   

    public static class SessionExtension
    {

        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {

            object data1 = JsonConvert.SerializeObject(value);

            session.SetString(key, JsonConvert.SerializeObject(value));
        }

    }
}