<html>
<body>

<?php
$con = mysql_connect("221.231.6.226:3306","a0825111004","323440");
if (!$con)
  {
  die('Could not connect: ' . mysql_error());
  }

mysql_select_db("a0825111004", $con);

$result = mysql_query("SELECT * FROM user_info");

echo "<table border='1'>
<tr>
<th>username</th>
<th>password</th>
<th>type</th>
<th>status</th>
</tr>";

while($row = mysql_fetch_array($result))
  {
  echo "<tr>";
  echo "<td>" . $row['username'] . "</td>";
  echo "<td>" . $row['password'] . "</td>";
  echo "<td>" . $row['type'] . "</td>";
  echo "<td>" . $row['status'] . "</td>";
  echo "</tr>";
  }
echo "</table>";

mysql_close($con);
?>
</body>
</html>