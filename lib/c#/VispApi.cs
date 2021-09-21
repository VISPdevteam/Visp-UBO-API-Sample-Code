using System;
using RestSharp; //106.11.7 ???
using Newtonsoft.Json; //version 13.0.1

public static class Globals {
    public const String VispApiURL = "https://data.visp.net";
}

namespace Visp {
    public class AuthResponse {
        public String token { get; set; }
    }
    public class Auth {
        String client_id;
        String client_secret;
        String token;
        String app_username;
        String app_password;

        public Auth() {
            this.client_id = ""; //add client id here
            this.client_secret = ""; //add client secret here
            this.app_username = ""; //add appuser username here
            this.app_password = ""; //add appuser password here
        }

        public Auth(String client_id, String client_secret, String uname, String pass) {
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.app_username = uname;
            this.app_password = pass;
        }

        public String getAppUsername() {
            return this.app_username;
        }
        public String getAppPassword() {
            return this.app_password;
        }
        public String getClientID() {
            return this.client_id;
        }
        public String getClientSecret() {
            return this.client_secret;
        }
        public String getToken() {
            if (String.IsNullOrEmpty(this.token)) {
                this.fetchToken();
            }
            return this.token;
        }

        public void setAppUsername(String app_username) {
            this.app_username = app_username;
        }
        public void setAppPassword(String app_password) {
            this.app_password = app_password;
        }
        public void setClientID(String client_id) {
            this.client_id = client_id;
        }
        public void setClientSecret(String client_secret) {
            this.client_secret = client_secret;
        }
        public void setToken(String token) {
            this.token = token;
        }

        public String fetchToken() {
            var client = new RestSharp.RestClient(this.getTokenUrl());
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(Method.GET);
            request.AddHeader("x-visp-username", this.app_username);
            request.AddHeader("x-visp-password", this.app_password);
            request.AddHeader("x-visp-client-id", this.client_id);
            request.AddHeader("x-visp-client-secret", this.client_secret);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            RestSharp.IRestResponse response = client.Execute(request);
            
            AuthResponse parsed = parseJson(response.Content);
            this.token = parsed.token;
            return this.token;
        }

        private AuthResponse parseJson (String json_str) {
            AuthResponse res = JsonConvert.DeserializeObject<AuthResponse>(json_str);
            return res;
        }

        private String getTokenUrl() {
            return Globals.VispApiURL + "/token";
        }
    }

    public class API {
        String query;
        String variables;
        String operationName;
        String token;
        public API() {
            this.query = "";
            this.variables = "";
            this.operationName = "";
            this.token = "";
        }
        public API(String token) {
            this.query = "";
            this.variables = "";
            this.operationName = "";
            this.token = token;
        }
        public API(String query, String vars, String name, String token) {
            this.query = query;
            this.variables = vars;
            this.operationName = name;
            this.token = token;
        }
        public String getQuery() {
            return this.query;
        }
        public String getVars() {
            return this.variables;
        }
        public String getName(){
            return this.operationName;
        }
        public String getToken() {
            return this.token;
        }
        public void setQuery(String query) {
            this.query = query;
        }
        public void setVars(String variables) {
            this.variables = variables;
        }
        public void setName(String operationName) {
            this.operationName = operationName;
        }
        public void setToken(String token) {
            this.token = token;
        }

        public dynamic SendRequest() {
            var client = new RestSharp.RestClient(this.getApiURL());
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(Method.POST);
            this.addHeaders(request);
            var query = this.getQueryString();
            request.AddParameter("application/json",query, ParameterType.RequestBody);
            RestSharp.IRestResponse response = client.Execute(request);

            dynamic resp = JsonConvert.DeserializeObject(response.Content);
            return resp;
        }

        private void addHeaders(RestSharp.RestRequest req) {
            if (String.IsNullOrEmpty(this.token)) {
                throw new Exception("Sending a request with no token");
            }
            req.AddHeader("authorization",this.token);
            req.AddHeader("Content-Type","application/json");
        }

        public String getQueryString() {
            var res = "{\"operationName\": " + (String.IsNullOrEmpty(this.operationName) ? "null," : ("\""+this.operationName+"\","));
            res += "\"variables\": " + (String.IsNullOrEmpty(this.variables) ? "null," : (this.variables + ","));
            res += "\"query\": " + "\"" + this.query + "\" }";
            return res;
        }

        public String getApiURL() {
            return Globals.VispApiURL + "/graphql";
        }
    }
}