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
using System.Collections;

namespace ImageSearch
{
   /// <summary>
   /// ImageViewControl.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class ImageViewControl : UserControl
   {
      DateTime mouseLastClick = DateTime.Now.AddSeconds(-1);
      Hashtable hashtable = new Hashtable();
      Data.DBHandler DB = new Data.DBHandler();

      public ImageViewControl()
      {
         InitializeComponent();
      }

      private void Btn_back_Click(object sender, RoutedEventArgs e)
      {
         viewPanel.Children.Clear();
         txtSearchBox.Text = "";
      }

      public void Btn_search_Click(object sender, RoutedEventArgs e)
      {
         viewPanel.Children.Clear();
         int count = (cbx_count.SelectedIndex + 1) * 10;
         if (!txtSearchBox.Text.Equals(""))
         {
            HttpGet(txtSearchBox.Text, count);
            DB.InsertLog(txtSearchBox.Text);
         }
         else
            MessageBox.Show("검색어를 입력해주세요.");
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

         reader.Close();
         respPostStream.Close();
         wRespFirst.Close();

         return true;
      }

      public void AddSearchedImage(string responseFromServer, int count)
      {
         var json = JObject.Parse(responseFromServer);
         var documents = json["documents"];
         
         for(int i=0; i < documents.Count(); i++)
         {
            BitmapImage bitmap = new BitmapImage(new Uri(documents[i]["image_url"].ToString()));
            Image image = new Image();
            image.AddHandler(MouseDownEvent, new RoutedEventHandler(Image_Click));
            image.Source = bitmap;
            hashtable.Add(image, bitmap);
            viewPanel.Children.Add(image);
         }
      }

      // 이미지 클릭 시 이미지 크게 보기
      public void Image_Click(object sender, RoutedEventArgs e)
      {
         Image image = new Image();
         // 현재 클릭된 이미지 소스를 가지고 옴
         image.Source = (BitmapImage)hashtable[sender];
         Window imageViewer = new ImageViewer(image);

         // 이미지 보기 창을 맨 위에 띄움
         imageViewer.Topmost = true;
         // 이미지 보기 창 띄우기
         imageViewer.Show();
      }
   }
}
