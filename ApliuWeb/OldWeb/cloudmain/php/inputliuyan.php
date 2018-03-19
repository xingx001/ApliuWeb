<html>
<body>
<?php
include '../../mysql/config.php';
$message=$_POST['message'];
date_default_timezone_set('Asia/Shanghai');
$date=date('Y-m-d H:i',time()+2*60);
$cur_username=$_COOKIE['cur_username'];

$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
  header("Location:../message.html?liuyan=false");
}

mysql_select_db($dbnamephp, $con);
//INSERT INTO Message (username,message,likenum,floor,date) VALUES('".$cur_username."','".$message."','1','00001','".$date."')
$sql="INSERT INTO Message (username,message,likenum,floor,date) VALUES('".$cur_username."','".$message."','0','00001','".$date."')";
mysql_query("set names 'utf8'");//写库
if (!mysql_query($sql,$con))
{
  header("Location:../message.html?liuyan=false");
}
else{
	header("Location:../message.html");
}

mysql_close($con)
?>
</body>
</html>