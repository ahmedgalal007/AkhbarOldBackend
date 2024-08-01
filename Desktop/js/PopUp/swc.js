$(document).ready(function() {	


  
 
		var id = '#dialog';
	
		//Get the screen height and width
		var maskHeight = $(document).height();
		var maskWidth = $(window).width();
	
        //Set heigth and width to mask to fill up the whole screen
		$('#popMask').css({ 'width': maskWidth, 'height': maskHeight });
		
		//transition effect		
		$('#popMask').fadeIn(500);	
		$('#popMask').fadeTo("slow",0.9);	
	
		//Get the window height and width
		var winH = $(window).height();
		var winW = $(window).width();
              
		//Set the popup window to center
		$(id).css('top',  winH/2-$(id).height()/2);
		$(id).css('left', winW/2-$(id).width()/2);
	
		//transition effect
		$(id).fadeIn(2000); 	
	
	//if close button is clicked
	$('.window .close').click(function (e) {
		//Cancel the link behavior
		e.preventDefault();
		
		$('#popMask').hide();
		$('.window').hide();
	});		
	
	//if popMask is clicked
	$('#popMask').click(function () {
		$(this).hide();
		$('.window').hide();
	});		
 }
);


setTimeout(function () {
    $('#dialog').hide();
}, 10000);
