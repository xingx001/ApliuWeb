<html>
<body>
<?php
include '../../mysql/config.php';
$isExist=$_GET['username'];
$boollogin=false;
$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
}

mysql_select_db($dbnamephp, $con);

$result = mysql_query("SELECT * FROM UserInfo");

while($row = mysql_fetch_array($result))
{
	if($isExist==$row['username'])
	{
		$boollogin=true;
	}
}
echo $boollogin;
mysql_close($con);
?>
</body>
</html>