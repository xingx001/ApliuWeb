using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace ApliuTools
{
    public class QRCode
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateCodeSimple(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置二维码编码格式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度大小  
            qrCodeEncoder.QRCodeScale = 4;
            //设置编码版本密封度
            qrCodeEncoder.QRCodeVersion = 8;
            //设置错误校验  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            Image image = qrCodeEncoder.Encode(content);
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(ms.ToArray());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            image.Dispose();
            return response;
        }
    }
}