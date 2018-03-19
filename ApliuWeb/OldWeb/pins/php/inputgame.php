<html>
<body>
<?php
include '../../mysql/config.php';
$score=$_GET['score'];
$game=$_GET['game'];
$time=$_GET['time'];
$max=$_GET['max'];
date_default_timezone_set('Asia/Shanghai');
$date=date('Y-m-d H:i',time()+2*60);
$cur_username=$_COOKIE['cur_username'];
$savegame="2";
if($cur_username!=null||$cur_username!="")
{
	$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
	if (!$con)
	{
	  //die('Could not connect: ' . mysql_error());
	  //header("Location:../message.html?liuyan=false");
	}
	mysql_select_db($dbnamephp, $con);
	
	$sqlselect="select * from Gamedata where username='". $cur_username ."' and game='pins' order by data desc limit 1";
	mysql_query("set names 'utf8'");//写库
	$result = mysql_query($sqlselect);
	while($row = mysql_fetch_array($result))
	{
		if($row['score']==$score&&$row['data']==$date){
			$savegame="3";
		}else{
			$sql="INSERT INTO Gamedata (username,game,score,max,usetime,data) VALUES('".$cur_username."','".$game."','".$score."','".$max."','".$time."','".$date."')";
			if (mysql_query($sql,$con)) $savegame="0";
			 
		}
	}
	mysql_close($con);
}else $savegame="1";
echo $savegame;
?>
</body>
</html>