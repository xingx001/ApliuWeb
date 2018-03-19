<html>
<body>
<?php
include '../../mysql/config.php';
$sql="";
$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
}

mysql_select_db($dbnamephp, $con);
$sql="UPDATE Other set count=count+1 where id='1'";

if (mysql_query($sql,$con))
{
}
mysql_close($con)
?>
</body>
</html>