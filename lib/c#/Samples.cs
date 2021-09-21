using System;
using Visp;

class Samples {
    public static void Customer() {
        //query a customer
        Visp.Auth auth = new Visp.Auth();
        var token = auth.getToken();
        Visp.API client = new Visp.API(token);

        var customerId = "1432780";
        client.setQuery("query customer($customer_id:Int!){\\n customer(customer_id:$customer_id){\\n customer_id\\n username\\n first_name\\n last_name\\n}\\n}");
        client.setVars("{\"customer_id\": " + customerId + "}");
        client.setName("customer");
        var response = client.SendRequest();

        var customer_id = response["data"]["customer"]["customer_id"];
        var username = response["data"]["customer"]["username"];
        Console.WriteLine("Customer_id: " + customer_id);
        Console.WriteLine("Username: " + username);
    }
    public static void CustomerWirelessSpeeds() {
        Visp.Auth auth = new Visp.Auth();
        var token = auth.getToken();
        Visp.API client = new Visp.API(token);

        var customerId = "1432780";
        client.setQuery("query customer($customer_id:Int!){\\n customer(customer_id:$customer_id){\\n package_instances {\\n package_id\\n package_name\\n service_instances{\\n service_details {\\n ... on ServiceTypeWifi{\\n service_number\\n username\\n up_speed\\n down_speed\\n up_speed_unit\\n down_speed_unit\\n}\\n}\\n}\\n}\\n}\\n}");
        client.setVars("{\"customer_id\": " + customerId + "}");
        client.setName("customer");
        var response = client.SendRequest();

        var packages = response["data"]["customer"]["package_instances"];
        foreach(dynamic package in packages) {
            var serviceInstances = package["service_instances"];
            foreach (dynamic serviceInstance in serviceInstances) {
                var serviceDetails = serviceInstance["service_details"];
                if (serviceDetails != null) {
                    var username = serviceDetails["username"];
                    var downSpeed = serviceDetails["down_speed"];
                    var downSpeedUnit = serviceDetails["down_speed_unit"];
                    var upSpeed = serviceDetails["up_speed"];
                    var upSpeedUnit = serviceDetails["up_speed_unit"];

                    Console.WriteLine("Service Username: " + username);
                    Console.WriteLine("Down Speed: " + downSpeed + " " + downSpeedUnit);
                    Console.WriteLine("Up Speed: " + upSpeed + " " + upSpeedUnit);
                }
            }
        }
    }
    public static void UpdateCustomer() {
        Visp.Auth auth = new Visp.Auth();
        var token = auth.getToken();
        Visp.API client = new Visp.API(token);

        var customerId = "1432780";
        client.setQuery("mutation updateCustomer($customer_id: Int!, $input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput) {\nupdateCustomer(customer_id: $customer_id, input_customer: $input_customer, input_customer_details: $input_customer_details){\\n customer_id\\n username\\n}\\n}");
        client.setVars("{\"customer_id\": "+customerId+",\"input_customer\": {\"username\": \"jkazama26\",\"password\": \"kazama123!\",\"first_name\": \"Jin\",\"last_name\": \"Kazama\",\"emails\": {\"email_address\": \"jinkazama@example.com\",\"description\": \"Example Email\"}},\"input_customer_details\": {\"bill_first_name\": \"Jin\",\"bill_last_name\": \"Kazama\",\"main_address1\": \"123 Street\",\"bill_address1\": \"123 Street\",\"main_address2\": \"\",\"bill_address2\": \"\",\"main_city\": \"New York\",\"bill_city\": \"New York\",\"main_state\": \"NY\",\"bill_state\": \"NY\",\"main_zip\": \"10001\",\"bill_zip\": \"10001\",\"main_phone1\": \"1234567890\",\"bill_phone1\": \"1234567890\"}}");
        client.setName("updateCustomer");
        var response = client.SendRequest();

        var customer = response["data"]["updateCustomer"];
        var customer_id = customer["customer_id"];
        var username = customer["username"];
        Console.WriteLine("Customer_id: " + customer_id);
        Console.WriteLine("Username: " + username);
    }
    public static void AddCustomer() {
        Visp.Auth auth = new Visp.Auth();
        var token = auth.getToken();
        Visp.API client = new Visp.API(token);

        client.setQuery("mutation addCustomer($input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput, $input_package_id: Int) {\\naddCustomer(input_customer: $input_customer, input_package_id: $input_package_id, input_customer_details: $input_customer_details){\\n customer_id\\n username\\n}\\n}");
        client.setVars("{\"input_customer\":{\"username\":\"jkazama27\",\"password\":\"kazama123!\",\"first_name\":\"Jin\",\"last_name\":\"Kazama\",\"emails\":{\"email_address\":\"jinkazama@gmail.com\",\"description\":\"Personal\"}},\"input_customer_details\":{\"bill_first_name\":\"Jin\",\"bill_last_name\":\"Kazama\",\"main_address1\":\"123 Street\",\"bill_address1\":\"123 Street\",\"main_address2\":\"\",\"bill_address2\":\"\",\"main_city\":\"New York\",\"bill_city\":\"New York\",\"main_state\":\"NY\",\"bill_state\":\"NY\",\"main_zip\":\"10001\",\"bill_zip\":\"10001\",\"main_phone1\":\"1234567890\",\"bill_phone1\":\"1234567890\",\"main_email\":\"jinkazama@gmail.com\",\"bill_email\":\"jinkazama@gmail.com\",\"bill_method\":\"Cash\"},\"input_package_id\":38915}");
        client.setName("addCustomer");
        var response = client.SendRequest();

        var customer = response["data"]["addCustomer"];
        var customer_id = customer["customer_id"];
        var username = customer["username"];
        Console.WriteLine("Customer_id: " + customer_id);
        Console.WriteLine("Username: " + username);
    }
}