var pingJS=function(){try{jQuery.getScript("http://h5.m.jd.com/active/track/mping.min.js",function(){var c=new MPing.inputs.PV();var b=new MPing();b.send(c)})}catch(a){}};var pingClick=function(d,h,f,c,a){if(d){try{var g=new MPing.inputs.Click(h);g.event_param=f;g.page_name=c;g.page_param=a;g.updateEventSeries();var b=new MPing();b.send(g)}catch(i){}}};