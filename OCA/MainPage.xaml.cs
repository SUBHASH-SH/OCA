using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;
using static OCA.OIViewModel1;
using Windows.Storage;
using System.Collections.Specialized;
using System.IO;
using System.Timers;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OCA
{

    public class OIViewModel1
    {
        

        public String CeInterpretation { get; set; }
        public String CeOi { get; set; }
        public String CeCoi { get; set; }
        public String CeVolume { get; set; }
        public String CePchng { get; set; }
        public String StrikePrice { get; set; }
        public String PePchng { get; set; }
        public String PeVolume { get; set; }
        public String PeCoi { get; set; }
        public String PeOi { get; set; }
        public String PeInterpretation { get; set; }

        public OIViewModel1(String CeInterpretation, String CeOi, String CeCoi, String CeVolume, String CePchng, String StrikePrice, String PePchng, String PeVolume, String PeCoi, String PeOi, String PeInterpretation)
        {
            this.CeInterpretation = CeInterpretation;
            this.CeOi = CeOi;
            this.CeCoi = CeCoi;
            this.CeVolume = CeVolume;
            this.CePchng = CePchng;
            this.StrikePrice = StrikePrice;
            this.PePchng = PePchng;
            this.PeVolume = PeVolume;
            this.PeCoi = PeCoi;
            this.PeOi = PeOi;
            this.PeInterpretation = PeInterpretation;
        }
    }

        /// <summary>
        /// An empty page that can be used on its own or navigated to within a Frame.
        /// </summary>
        public sealed partial class MainPage : Page
    {
        String url = "https://www.nseindia.com/api/option-chain-indices?symbol=NIFTY";
        // how to get response form ssl encreption
        int inc = 0;
        Timer timer = new Timer();
        private List<OIViewModel1> OIViewModel { get; set; }
        

        public MainPage()
        {
            
            this.InitializeComponent();

            // timer to call MyMethod() every minutes 
            //for (int i = 0;i < 5;i++) 
            //{

            //   inc++;
            GetData();
            

            

            //}

        }

        private async void updateOiData()
        {

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync("myconfig0.json");
            string file2 = await FileIO.ReadTextAsync(sampleFile);
            List<Filtered> FilteredData2 = JsonConvert.DeserializeObject<List<Filtered>>(file2);
            //foreach (Filtered flr in FilteredData2)
            //{
            //OIViewModel = new List<OIViewModel1> { new OIViewModel1("sdf", "+500", "+200", "24562", "45", "19000 ", "20", "205040", "+200", "+800", "Short Buildup") };
            //}

            OIViewModel = new List<OIViewModel1>();
            for (int i = 0;i < 4;i++) 
            {
                OIViewModel1 oIViewModel = new OIViewModel1(i.ToString(), "+500", "+200", "24562", "45", "19000 ", "20", "205040", "+200", "+800", "Short Buildup");
                
                OIViewModel.Add(oIViewModel);
            }
            dataGrid1.ItemsSource = OIViewModel;
        }

        public async void GetData() 
        {
            try {
                
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
                
                HttpResponseMessage response = client.GetAsync(url).Result;

               

                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response as a string
                    var requestUri = response.RequestMessage.RequestUri;
                    HttpResponseMessage responseHeadders = client.GetAsync(requestUri).Result;

                    string responseBody = responseHeadders.Content.ReadAsStringAsync().Result;
                    

                    // Parse the JSON string into a JObject
                    JObject jsonObject = JObject.Parse(responseBody);

                    // Extract the string value from the JSON 
                    JToken expiryDates = jsonObject.SelectToken("records.expiryDates");
                    JToken data = jsonObject.SelectToken("records.data");
                    JToken timestamp = jsonObject.SelectToken("records.timestamp");
                    JToken underlyingvalue = jsonObject.SelectToken("records.underlyingValue");
                    JToken strikeprices = jsonObject.SelectToken("records.strikePrices");

                    JToken filtered = jsonObject.SelectToken("filtered.data");
                    JToken cE = jsonObject.SelectToken("filtered.CE.totOI");
                    JToken pE = jsonObject.SelectToken("filtered.PE.totOI");

                    string expiryDate = expiryDates.ToString();             // use it Directly.
                    string dataData = data.ToString();                      
                    string timeStamp = timestamp.ToString();               // use it Directly.
                    double number = double.Parse(underlyingvalue.ToString());
                    int underlyingVal = (int)number;
                    string strikePrices = strikeprices.ToString();  // use it Directly.

                    IndexText.Text = underlyingVal.ToString();

                    string filteredData = filtered.ToString();
                    double cEData = double.Parse(cE.ToString());          // use it Directly.
                    double pEData = double.Parse(pE.ToString());          // use it Directly.

                    //Sentiment Analysis
                    double pcr = pEData / cEData;
                    String pcrValue = Math.Round(pcr, 2).ToString();

                    pcrText.Text = "PCR - " + pcrValue;

                    Model.expiryDate = expiryDate;



                    List<Datum> DatumData = JsonConvert.DeserializeObject<List<Datum>>(dataData);
                    List<Filtered> FilteredData = JsonConvert.DeserializeObject<List<Filtered>>(filteredData);


                    List<Filtered> getCE0 = new List<Filtered>();
                    List<Filtered> getCE1 = new List<Filtered>();
                    List<Filtered> getCE2 = new List<Filtered>();
                    List<Filtered> getCE3 = new List<Filtered>();
                    List<Filtered> getCE4 = new List<Filtered>();
                    List<Filtered> getCE5 = new List<Filtered>();
                    List<Filtered> getCE6 = new List<Filtered>();
                    List<Filtered> getCE7 = new List<Filtered>();
                    List<Filtered> getPE = new List<Filtered>();


                    

                    int count = 0;

                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("myconfig"+inc+ ".json");
                    
                    await FileIO.AppendTextAsync(file, "[");
                    
                    //for first time
                    foreach (Filtered str in FilteredData) 
                    {
                        

                        if (str.strikePrice > underlyingVal - 400) //18,398
                        {

                            if (count < 17)
                            {

                                string json = JsonConvert.SerializeObject(str);
                                // write string to a file
                                await FileIO.AppendTextAsync(file, json);
                                

                                if (count < 16) {
                                   await FileIO.AppendTextAsync(file, ",");
                                    
                                };

                                Debug.WriteLine(str.strikePrice);

                            }
                            
                            count++;
                        }
                        

                    }
                    await FileIO.AppendTextAsync(file, "]");
                    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                    StorageFile sampleFile = await storageFolder.GetFileAsync("myconfig0.json");
                    string file2 = await FileIO.ReadTextAsync(sampleFile);
                    List<Filtered> FilteredData2 = JsonConvert.DeserializeObject<List<Filtered>>(file2);
                    List<OIViewModel1> models2 = new List<OIViewModel1>();
                    foreach (Filtered flr in FilteredData2)
                    {
                        

                    }


                }
            }
            catch(Exception ex) 
            { 
                Debug.WriteLine("Exception Thrown :" +ex); 
            }
        }

        private void updateData_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            updateOiData();
        }
    }

}
