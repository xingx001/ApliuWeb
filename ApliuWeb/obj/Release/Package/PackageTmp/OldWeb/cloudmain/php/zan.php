<html>
<body>
<?php
include '../../mysql/config.php';
$id=$_GET['id'];
$issuccess="1";
$sql="";
$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
  //header("Location:../message.html?liuyan=false");
}

mysql_select_db($dbnamephp, $con);
//INSERT INTO Message (username,message,likenum,floor,date) VALUES('".$cur_username."','".$message."','1','00001','".$date."')
$sql="UPDATE Message set likenum=likenum+1 where id='". $id ."'";

if (mysql_query($sql,$con))
{
  $issuccess=0;
}
echo $issuccess;
mysql_close($con)
?>
</body>
</html>