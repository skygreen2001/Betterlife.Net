$(function(){ 
  $("#survey").click(function(){        
    $.get("HttpData/Survey.ashx", function(result){  
        $("#like_language").html(result);
    }); 
  });
});