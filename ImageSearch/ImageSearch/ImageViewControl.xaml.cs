using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;

namespace ImageSearch
{
   /// <summary>
   /// ImageViewControl.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class ImageViewControl : UserControl
   {
      public ImageViewControl()
      {
         InitializeComponent();
      }

      private void Btn_back_Click(object sender, RoutedEventArgs e)
      {
         viewPanel.Children.Clear();
      }

      public void Btn_search_Click(object sender, RoutedEventArgs e)
      {
         viewPanel.Children.Clear();
         HttpGet(txtSearchBox.Text, 10);
         
      }
      
      public bool HttpGet(string POI, int count)
      {
         string app_key = "bfad53875abbf0b4112cb53a3c24e202";
         string header = "KakaoAK " + app_key;

         string Url = string.Format("https://dapi.kakao.com/v2/search/image");
         StringBuilder getParams = new StringBuilder();
         getParams.Append("?query=" + POI);

         HttpWebRequest wReqFirst = (HttpWebRequest)WebRequest.Create(Url+getParams+"&size="+count);
         wReqFirst.Headers.Add("Authorization", header);
         wReqFirst.ContentType = "application/json; charset=utf-8";
         wReqFirst.Method = "GET";
         wReqFirst.ServicePoint.Expect100Continue = false;

         HttpWebResponse wRespFirst = (HttpWebResponse)wReqFirst.GetResponse();
         Stream respPostStream = wRespFirst.GetResponseStream();
         StreamReader reader = new StreamReader(respPostStream, Encoding.GetEncoding("EUC-KR"), true);
         string responseFromServer = reader.ReadToEnd();

         AddSearchedImage(responseFromServer, count);

        // MessageBox.Show(responseFromServer);

         reader.Close();
         respPostStream.Close();
         wRespFirst.Close();

         return true;
      }

      public void AddSearchedImage(string responseFromServer, int count)
      {
         var json = JObject.Parse(responseFromServer);
         var documents = json["documents"];
         for(int i=0;i<count;i++)
         {
            BitmapImage bitmap = new BitmapImage(new Uri(documents[i]["image_url"].ToString()));
            Image image = new Image();
            image.Source = bitmap;
            viewPanel.Children.Add(image);
         }
      }
   }
}
