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
using System.Threading;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Microsoft.Toolkit.Uwp.UI.Controls.Primitives;
using Windows.UI.Composition;
using System.Collections.ObjectModel;


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
        int incData = 0;
        int underlyingVal;
        private DispatcherTimer timer;

        private List<OIViewModel1> OIViewModel { get; set; }
        


        public MainPage()
        {

            this.InitializeComponent();
            selectOption.Items.Add("NIFTY");
            selectOption.Items.Add("FIN NIFTY");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Start the timer
            timer.Start();


        }

        private async Task updateOiData()
        {

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile1 = await storageFolder.GetFileAsync(incData + ".json");
            string file1 = await FileIO.ReadTextAsync(sampleFile1);
            List<Filtered> FilteredData1 = JsonConvert.DeserializeObject<List<Filtered>>(file1);

           
            String TimeCreation = sampleFile1.DateCreated.ToString();

            StorageFile sampleFile2 = await storageFolder.GetFileAsync(incData - 1 + ".json");
            string file2 = await FileIO.ReadTextAsync(sampleFile2);
            List<Filtered> FilteredData2 = JsonConvert.DeserializeObject<List<Filtered>>(file2);

            OIViewModel = new List<OIViewModel1>();

            foreach (var Data in FilteredData1.Zip(FilteredData2, (a, b) => new { A = a, B = b }))
            {

                Filtered data1 = Data.A;
                Filtered data2 = Data.B;

                String CeInterpretation = "NOT SURE";
                String PeInterpretation = "NOT SURE";

                //1 is the current data.
                //2 is the older data.

                if ((data1.CE.strikePrice > underlyingVal - 250) && (data1.CE.strikePrice == data2.CE.strikePrice))
                {
                    int CeOi1 = (int)data1.CE.openInterest, CeCoi1 = (int)data1.CE.changeinOpenInterest,
                            CeVolume1 = data1.CE.totalTradedVolume, CePchng1 = (int)data1.CE.lastPrice,
                            PePchng1 = (int)data1.PE.lastPrice, PeVolume1 = data1.PE.totalTradedVolume,
                            PeCoi1 = (int)data1.PE.changeinOpenInterest, PeOi1 = (int)data1.PE.openInterest;

                    int CeOi2 = (int)data2.CE.openInterest, CeCoi2 = (int)data2.CE.changeinOpenInterest,
                                CeVolume2 = data2.CE.totalTradedVolume, CePchng2 = (int)data2.CE.lastPrice,
                                PePchng2 = (int)data2.PE.lastPrice, PeVolume2 = data2.PE.totalTradedVolume,
                                PeCoi2 = (int)data2.PE.changeinOpenInterest, PeOi2 = (int)data2.PE.openInterest;

                    if (CePchng1 > CePchng2 && CeOi1 > CeOi2)
                    {
                        CeInterpretation = "Long Call Buildup";
                    }
                    else if (CePchng1 < CePchng2 && CeOi1 > CeOi2)
                    {
                        CeInterpretation = "Call short Buildup";
                    }
                    else if (CePchng1 > CePchng2 && CeOi1 < CeOi2)
                    {
                        CeInterpretation = "Short Covering";
                    }
                    else if (CePchng1 < CePchng2 && CeOi1 < CeOi2)
                    {
                        CeInterpretation = "Call Writing";
                    }

                    if (PePchng1 > PePchng2 && PeOi1 > PeOi2)
                    {
                        PeInterpretation = "Long Call Buildup";
                    }
                    else if (PePchng1 < PePchng2 && PeOi1 > PeOi2)
                    {
                        PeInterpretation = "Call short Buildup";
                    }
                    else if (PePchng1 > PePchng2 && PeOi1 < PeOi2)
                    {
                        PeInterpretation = "Short Covering";
                    }
                    else if (PePchng1 < PePchng2 && PeOi1 < PeOi2)
                    {
                        PeInterpretation = "Call Writing";
                    }



                    OIViewModel1 oIViewMode1 = new OIViewModel1("", CeOi1.ToString(),
                                                                                  CeCoi1.ToString(),
                                                                                  CeVolume1.ToString(),
                                                                                  CePchng1.ToString(),
                                                                                  data1.CE.strikePrice.ToString(),
                                                                                  PePchng1.ToString(),
                                                                                  PeVolume1.ToString(),
                                                                                  PeCoi1.ToString(),
                                                                                  PeOi1.ToString(),
                                                                  "");
                    OIViewModel.Add(oIViewMode1);


                    OIViewModel1 oIViewMode2 = new OIViewModel1(CeInterpretation, (CeOi1 - CeOi2).ToString(),
                                                                                  (CeCoi1 - CeCoi2).ToString(),
                                                                                  (CeVolume1 - CeVolume2).ToString(),
                                                                                  (CePchng1 - CePchng2).ToString(),
                                                                                  data1.CE.strikePrice.ToString(),
                                                                                  (PePchng1 - PePchng2).ToString(),
                                                                                  (PeVolume1 - PeVolume2).ToString(),
                                                                                  (PeCoi1 - PeCoi2).ToString(),
                                                                                  (PeOi1 - PeOi2).ToString(),
                                                                  PeInterpretation);
                    OIViewModel.Add(oIViewMode2);




                }
                else
                {
                    updateDataBtn.Content = "RE-TRY";
                }
                updateDataBtn.Content = "UPDATE";

                if (data1.CE.strikePrice > underlyingVal + 250) { break; }

            }

            dataGrid1.ItemsSource = OIViewModel;
            
        }

        public async Task GetData()
        {
            try
            {

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
                    //JToken expiryDates = jsonObject.SelectToken("records.expiryDates");
                    JToken data = jsonObject.SelectToken("records.data");
                    JToken timestamp = jsonObject.SelectToken("records.timestamp");
                    JToken underlyingvalue = jsonObject.SelectToken("records.underlyingValue");
                    JToken strikeprices = jsonObject.SelectToken("records.strikePrices");

                    JToken filtered = jsonObject.SelectToken("filtered.data");
                    JToken cE = jsonObject.SelectToken("filtered.CE.totOI");
                    JToken pE = jsonObject.SelectToken("filtered.PE.totOI");

                    //string expiryDate = expiryDates.ToString();             // use it Directly.
                    /*string dataData = data.ToString();                      
                    string timeStamp = timestamp.ToString();               // use it Directly.
                    string strikePrices = strikeprices.ToString();  // use it Directly.*/

                    while (underlyingvalue.ToString() == null) {
                        underlyingvalue = jsonObject.SelectToken("records.underlyingValue");
                    }

                    double number = double.Parse(underlyingvalue.ToString());
                    underlyingVal = (int)number;
                    IndexText.Text = underlyingVal.ToString();

                    string filteredData = filtered.ToString();
                    double cEData = double.Parse(cE.ToString());          // use it Directly.
                    double pEData = double.Parse(pE.ToString());          // use it Directly.

                    //Sentiment Analysis
                    double pcr = pEData / cEData;
                    String pcrValue = Math.Round(pcr, 2).ToString();

                    pcrText.Text = "PCR - " + pcrValue;

                    //Model.expiryDate = expiryDate;

                    List<Filtered> FilteredData = JsonConvert.DeserializeObject<List<Filtered>>(filteredData);

                    int count = 0;

                    //String todaysDate = DateTime.Now.ToString().Replace(':','-');

                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(incData + ".json");
                    

                    await FileIO.AppendTextAsync(file, "[");

                    //for first time
                    foreach (Filtered str in FilteredData)
                    {


                        if (str.strikePrice > 18000) //take all strike prices between 18000 to 21000
                        {

                            if (count < 60)
                            {

                                string json = JsonConvert.SerializeObject(str);
                                // write string to a file
                                await FileIO.AppendTextAsync(file, json);


                                if (count < 59)
                                {
                                    await FileIO.AppendTextAsync(file, ",");

                                };

                                Debug.WriteLine(str.strikePrice);

                            }
                            if (str.strikePrice == 21000) { break; }

                            count++;
                        }


                    }
                    await FileIO.AppendTextAsync(file, "]");
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Thrown :" + ex);
            }
        }

        public async void ExecuteMethodsAsync()
        {
            await GetData(); // Wait for the first method to complete
            if (incData > 0)
            {
                await updateOiData(); // Wait for the second method to complete after the first one
            }
            incData++;
        }

        private void updateData_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ExecuteMethodsAsync();
            DateTime dateTime = DateTime.Now;
            lastUpdateText.Text = dateTime.ToString("HH:mm:ss");

        }
        private void Timer_Tick(object sender, object e)
        {
            // Update the clock label with the current time
            DateTime CTime = DateTime.Now;
            currentTime.Text = CTime.ToString("HH:mm:ss");
        }

        private void updateAIO_Data_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
