<html>
<body>
<?php
include '../../mysql/config.php';
$username=$_POST['username'];
$password=$_POST['password'];
$id=$_GET['id'];
$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
  header("Location:../register.html?register=false");
}

mysql_select_db($dbnamephp, $con);

$sql="INSERT INTO UserInfo (username, password, type,status) VALUES('".$username."','".$password."','Normal','1')";
mysql_query("set names 'utf8'");//写库
if (!mysql_query($sql,$con))
{
  header("Location:../register.html?register=false");
}
else{
	header("Location:../login.html?register=true&id=" . $id);
}

mysql_close($con)
?>
</body>
</html>