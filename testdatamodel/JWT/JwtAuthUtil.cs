﻿using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace testdatamodel.JWT
{
    public class JwtAuthUtil
    {
        /// <summary>
        /// 產生Token
        /// </summary>
        /// <param name="id">使用者id</param>
        /// <returns></returns>
        public string GenerateToken(int id)
        {
            string secret = "kirukiru";//加解密的key,如果不一樣會無法成功解密 (可以修改可以自創)
            Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            claim.Add("Id", id);
            claim.Add("iat", DateTime.Now.ToString());//建立時間
            claim.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("86400")).ToString());//Token 時效設定100秒
            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS512);//產生token
            return token;
        }
        /// <summary>
        /// 查詢使用者ID
        /// </summary>
        /// <param name="Token">輸入前面拿到的Token key</param>
        /// <returns></returns>
        public static int GetId(string Token)
        {
            string secret = "ILoveRocketCoding";//加解密的key,如果不一樣會無法成功解密
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                Token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS512);
            return Convert.ToInt32(jwtObject["Id"]);
        }
    }
}