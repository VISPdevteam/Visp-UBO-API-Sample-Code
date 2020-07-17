const functions = require('./functions');

const main = async() => {
  token = await functions.getToken();

  data = {
    operationName: 'addCustomer',
    query: 'mutation addCustomer($input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput, $input_package_id: Int) {\n  addCustomer(input_customer: $input_customer, input_package_id: $input_package_id, input_customer_details: $input_customer_details){\n    customer_id\n    username\n}} ',
    variables: {
        input_customer: {
            username: "jkazama27",
            password: "kazama123!",
            first_name: "Jin",
            last_name: "Kazama",
            emails: {
                email_address: "jinkazama@example.com",
                description: "personal"
            }
        },
        input_customer_details: {
            bill_first_name: "Jin",
            bill_last_name: "Kazama",
            main_address1: "123 Street",
            bill_address1: "123 Street",
            main_address2: "",
            bill_address2: "",
            main_city: "New York",
            bill_city: "New York",
            main_state: "NY",
            bill_state: "NY",
            main_zip: "10001",
            bill_zip: "10001",
            main_phone1: "1234567890",
            bill_phone1: "1234567890",
            bill_method: "Cash"
        },
        input_package_id: 38915
    }
  };

  response = await functions.sendRequest(data, token);

  //extract data
  if (response.errors && response.errors.length){
    console.log(response.errors);
  } else {
    data = response.data;
    customer = data.addCustomer;
    console.log("Customer ID: "+customer.customer_id);
  }
}

main();
