using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;

class HttpPost
{
    public HttpPost(string postUri)
    {
        this.PostUri = postUri;
    }

    private WebProxy _proxy;
    public WebProxy Proxy
    {
        get { return _proxy; }
        set { _proxy = value; }
    }

    private string _postUri;
    public string PostUri
    {
        get { return _postUri; }
        set { _postUri = value; }
    }

    private string _language;
    public string Language
    {
        get { return _language; }
        set { _language = value; }
    }

    public event EventHandler PostComplete;

    public void doPost(PostParamCollection postParamCollection)
    {
        try
        {
            Uri uri = new Uri(this.PostUri);
            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, this.Language);

            if (this.Proxy != null)
                webRequest.Proxy = Proxy;

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            string parameterString = "";

            foreach (PostParam parameter in postParamCollection)
            {
                parameterString += parameter.Paramter + "=" + parameter.Value + "&";
            }

            parameterString = parameterString.Substring(0, parameterString.Length - 1);

            byte[] byteArray = Encoding.UTF8.GetBytes(parameterString);
            webRequest.ContentLength = byteArray.Length;

            Stream stream = webRequest.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            WebResponse webResponse = webRequest.GetResponse();
            stream = webResponse.GetResponseStream();

            StreamReader streamReader = new StreamReader(stream);
            string responseStream = streamReader.ReadToEnd();

            webResponse.Close();
            streamReader.Close();

            if (PostComplete != null)
            {
                PostComplete.Invoke(responseStream, null);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public void doPost(PostParam parameter)
    {
        try
        {
            Uri uri = new Uri(this.PostUri);
            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, this.Language);

            if (this.Proxy != null)
                webRequest.Proxy = Proxy;

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            string parameterString = "";

            parameterString += parameter.Paramter + "=" + parameter.Value + "&";

            parameterString = parameterString.Substring(0, parameterString.Length - 1);

            byte[] byteArray = Encoding.UTF8.GetBytes(parameterString);
            webRequest.ContentLength = byteArray.Length;

            Stream stream = webRequest.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            WebResponse webResponse = webRequest.GetResponse();
            stream = webResponse.GetResponseStream();

            StreamReader streamReader = new StreamReader(stream);
            string responseStream = streamReader.ReadToEnd();

            webResponse.Close();
            streamReader.Close();

            if (PostComplete != null)
            {
                PostComplete.Invoke(responseStream, null);
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}

class PostParam
{
    public PostParam() { }

    public PostParam(string paramter, string value)
    {
        this.Paramter = paramter;
        this.Value = value;
    }

    private string _paramter;
    public string Paramter
    {
        get { return _paramter; }
        set { _paramter = value; }
    }

    private string _value;
    public string Value
    {
        get { return _value; ; }
        set { _value = value; }
    }
}

class PostParamCollection : List<PostParam>
{
}
