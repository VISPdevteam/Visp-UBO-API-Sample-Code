const functions = require('./functions');

const main = async() => {
  token = await functions.getToken();

  data = {
    operationName: 'updateCustomer',
    query: 'mutation updateCustomer($customer_id: Int!, $input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput) {\n  updateCustomer(customer_id: $customer_id, input_customer: $input_customer, input_customer_details: $input_customer_details){\n    customer_id\n   username\n    first_name\n    last_name\n}\n}',
    variables: {
        customer_id: 1432780,
        input_customer: {
            username: "jkazama26",
            password: "kazama123!",
            first_name: "Jin",
            last_name: "Kazama",
            emails: {
                email_address: "jinkazama@example.com",
                description: "Example Email"
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
        },
    }
  };

  response = await functions.sendRequest(data, token);

  //extract data
  if (response.errors && response.errors.length){
    console.log(response.errors);
  } else {
    data = response.data;
    customer = data.updateCustomer;
    console.log("Customer ID: "+customer.customer_id);
    console.log("Username: "+customer.username);
  }
}

main();
