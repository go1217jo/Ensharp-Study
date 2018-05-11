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
using System.Text.RegularExpressions;

namespace ImageSearch
{
   /// <summary>
   /// ImageViewControl.xaml에 대한 상호 작용 논리
   /// </summary>
   public partial class ImageViewControl : UserControl
   {
      // 처음 클릭시간 저장
      DateTime mouseLastClick = DateTime.Now.AddSeconds(-1);

      Hashtable hashtable = new Hashtable();
      Data.DBHandler DB;
      Window imageViewer;

      public ImageViewControl(Data.DBHandler DB)
      {
         InitializeComponent();
         this.DB = DB;
         txtSearchBox.KeyDown += new KeyEventHandler(txt_KeyDown);
         txtSearchBox.Focus();
      }

      public void txt_KeyDown(object sender, KeyEventArgs e)
      {
         if(e.Key == Key.Enter)
            Btn_search_Click(sender, e);
      }

      public void Btn_search_Click(object sender, RoutedEventArgs e)
      {
         viewPanel.Children.Clear();
         int count = (cbx_count.SelectedIndex + 1) * 10;

         if (!(txtSearchBox.Text.Equals("") || txtSearchBox.Text.Contains("\\") || txtSearchBox.Text.Contains("/") || txtSearchBox.Text.Contains(".")))
         {
            HttpGet(txtSearchBox.Text, count);
            // 검색했던 키워드가 없어 갱신에 실패했다면 로그 추가
            if (!DB.UpdateTime(txtSearchBox.Text))
               DB.InsertLog(txtSearchBox.Text);
         }
         else
            MessageBox.Show("올바른 검색어를 입력해주세요.");
      }
      
      public bool HttpGet(string POI, int count)
      {
         string app_key = "bfad53875abbf0b4112cb53a3c24e202";
         string header = "KakaoAK " + app_key;

         string Url = string.Format("https://dapi.kakao.com/v2/search/image");
         StringBuilder getParams = new StringBuilder();
         getParams.Append("?query=" + POI);

         HttpWebRequest wReqFirst = (HttpWebRequest)WebRequest.Create(Url+getParams + "&size=" + count);
         wReqFirst.Headers.Add("Authorization", header);
         wReqFirst.ContentType = "application/json; charset=utf-8";
         wReqFirst.Method = "GET";
         wReqFirst.ServicePoint.Expect100Continue = false;

         HttpWebResponse wRespFirst = (HttpWebResponse)wReqFirst.GetResponse();
         Stream respPostStream = wRespFirst.GetResponseStream();
         StreamReader reader = new StreamReader(respPostStream, Encoding.GetEncoding("EUC-KR"), true);
         string responseFromServer = reader.ReadToEnd();

         AddSearchedImage(responseFromServer);

         reader.Close();
         respPostStream.Close();
         wRespFirst.Close();

         return true;
      }

      public void AddSearchedImage(string responseFromServer)
      {
         var json = JObject.Parse(responseFromServer);
         var documents = json["documents"];

         if (documents.Count() == 0)
            MessageBox.Show("검색결과가 없습니다.");

         // 이미지를 로드하고 이미지 패널에 출력한다
         for (int idx = 0; idx < documents.Count(); idx++)
         {
            BitmapImage bitmap = new BitmapImage(new Uri(documents[idx]["image_url"].ToString()));
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
         if ((DateTime.Now - mouseLastClick).Milliseconds < 200)
         {
            Image image = new Image();
            // 현재 클릭된 이미지 소스를 가지고 옴
            image.Source = ((BitmapImage)hashtable[sender]).Clone();
            image.AddHandler(MouseDownEvent, new RoutedEventHandler(Image_Close_Click));
            imageViewer = new ImageViewer(image);
         
            // 이미지 보기 창을 맨 위에 띄움
            imageViewer.Topmost = true;
            // 이미지 보기 창 띄우기
            imageViewer.Show();

            mouseLastClick = DateTime.Now.AddSeconds(-1);
         }
         else
            mouseLastClick = DateTime.Now;
      }

      public void Image_Close_Click(object sender, RoutedEventArgs e)
      {
         imageViewer.Close();
      }
   }
}
