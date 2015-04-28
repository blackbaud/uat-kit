$(document).ready(function(){
  $('ul.nav li').on('mouseover', function(){
    $(this).animate({fontSize: "20px"}, 500);
  });
  
  $('ul.nav li').on('mouseout', function(){
    $(this).animate({fontSize: "16px"}, 500);
  });
});