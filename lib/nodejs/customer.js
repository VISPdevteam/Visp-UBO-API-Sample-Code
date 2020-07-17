const functions = require('./functions');

const main = async() => {
  token = await functions.getToken();
  
  data = {
    operationName: 'customer',
    query: 'query customer($customer_id:Int!){\n customer(customer_id:$customer_id){\n customer_id\n username\n first_name\n last_name\n}\n}',
    variables: {
      customer_id: 1432780
    }
  };

  response = await functions.sendRequest(data, token);

  //extract data
  if (response.errors && response.errors.length){
    console.log(response.errors);
  } else {
    data = response.data;
    customer = data.customer;
    console.log("Customer ID: "+customer.customer_id);
    console.log("Username: "+customer.username);
  }
}

main();
