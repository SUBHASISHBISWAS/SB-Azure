    function Display(){
        var context = getContext();        
        var response = context.getResponse();

        response.setBody("This is a stored procedure");
    }
