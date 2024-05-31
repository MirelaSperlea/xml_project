<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html>
      <head>
        <title>Table of Bicycle Rentals</title>
        <style>
          table {
            border-collapse: collapse;
            width: 100%;
          }
          th, td {
            border: 1px solid black;
            padding: 8px;
            text-align: left;
          }
          th {
            background-color: #f2f2f2;
          }
        </style>
      </head>
      <body>
        <h1>Table of Bicycle Rentals</h1>
        <table>
          <tr>
            <th>Id</th>
            <th>Model</th>
            <th>Color</th>
            <th>Weight</th>
            <th>Price per Hour</th>
            <th>Availability</th>
            <th>Client Name</th>
            <th>Phone</th>
            <th>Email</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th>Duration</th>
            <th>Cost</th>
          </tr>
          <xsl:apply-templates select="inchirieri_biciclete/inchiriere"/>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="inchiriere">
    <tr>
      <td><xsl:value-of select="@id"/></td>
      <td><xsl:value-of select="bicicleta/model"/></td>
      <td><xsl:value-of select="bicicleta/caracteristici/culoare"/></td>
      <td><xsl:value-of select="bicicleta/caracteristici/greutate"/></td>
      <td><xsl:value-of select="bicicleta/pret_pe_ora"/></td>
      <td><xsl:value-of select="bicicleta/disponibilitate"/></td>
      <td><xsl:value-of select="client/nume"/></td>
      <td><xsl:value-of select="client/contact/telefon"/></td>
      <td><xsl:value-of select="client/contact/email"/></td>
      <td><xsl:value-of select="perioada/inceput"/></td>
      <td><xsl:value-of select="perioada/sfarsit"/></td>
      <td><xsl:value-of select="perioada/durata"/></td>
      <td><xsl:value-of select="suma"/></td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
