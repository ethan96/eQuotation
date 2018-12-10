<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuoteApproval_new.aspx.vb" Inherits="EDOC.QuoteApproval_new" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Advantech eQuotation</title>
    <link href="Style/bootstrap.min.css" rel="stylesheet" />
    <link href="Style/QuoteApproval_new.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-xs-12 col-sm-6">
                    <img id="imgLogo1" src="http://172.20.1.30:8300/Images/logo.GIF" class="img-responsive" />
                </div>
                <div class="col-xs-12 col-sm-6">
                    <div class="col-xs-12 col-sm-12 col-md-6">
                        <span class="glyphicon glyphicon-user"></span>
                        <span id="lbUID">wenyu.lai@advantech-uk.com</span>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-6">
                        <strong>Quote No：</strong>
                        <a id="HyperLinkQuoteNo" href="QuotationMaster.aspx?UID=fec7426b5acb431">GQ046183</a>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-offset-6 col-md-6">
                        <ul class="list-inline">
                            <li><a id="hyEQHome" href="../Home.aspx" class="text-warning">Home</a></li>
                            <li><a id="IdMyAdvantech" href="javascript:__doPostBack('ctl00$IdMyAdvantech','')" class="text-warning">MyAdvantech</a></li>
                            <li><a id="lbLogOut" href="javascript:__doPostBack('ctl00$lbLogOut','')" class="text-muted">Logout</a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="row hidden-xs">
                <div class="col-xs-12 col-sm-6 col-md-9">
                    <img id="imgLogo2" src="http://eq.advantech.com/Images/LogoPi.jpg" class="img-responsive margin-top-10" />
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 font-size-8">
                    <ul class="list-unstyled">
                        <li>Ekkersrijt 5708 Science Park</li>
                        <li>Eindhoven 5692 EP Son </li>
                        <li>The Netherlands</li>
                        <li>Tel:31-40-2677000</li>
                    </ul>
                </div>
            </div>
            <div class="block_item">
                <p class="block"><strong>Quote Description：</strong>For RWD Test</p>
            </div>
            <div class="block_item">
                <p><strong>Quote No：</strong>GQ046183</p>
            </div>
            <div class="block_item">
                <p><strong>Created By：</strong>emil.hsu@advantech.de</p>
            </div>
            <div class="block_item">
                <p><strong>Quote Date：</strong>1/22/2016 10:19:47 AM</p>
            </div>
            <div class="block_item">
                <p><strong>Prepared By：</strong>&nbsp;</p>
            </div>
            <div class="block_item">
                <p><strong>Quote To：</strong>INRS(EFFRIN12)</p>
            </div>
            <div class="block_item">
                <p><strong>Delivery Date：</strong>2/20/2016</p>
            </div>
            <div class="block_item">
                <p><strong>Expired Date：</strong>2/20/2016</p>
            </div>
            <div class="block_item">
                <p><strong>Shipping Terms：</strong>EX Works</p>
            </div>
            <div class="block_item">
                <p><strong>Payment Terms：</strong>PPD</p>
            </div>
            <div class="block_item">
                <p><strong>Freight：</strong>TBD</p>
            </div>
            <div class="block_item">
                <p><strong>Insurance：</strong>0.00</p>
            </div>
            <div class="block_item">
                <p><strong>Special Charge：</strong>0.00</p>
            </div>
            <div class="block_item">
                <p><strong>Tax Rate：</strong>0.00</p>
            </div>
            <div class="margin-20">
                <p><strong>Account Info：</strong></p>
            </div>
            <div class="block_item">
                <p>Office：&nbsp;</p>
            </div>
            <div class="block_item">
                <p>Currency：EUR</p>
            </div>
            <div class="block_item">
                <p>Sales Email：vincent.coulon@advantech.fr</p>
            </div>
            <div class="block_item">
                <p>Direct Phone：&nbsp;</p>
            </div>
            <div class="block_item">
                <p>Attention：&nbsp;</p>
            </div>
            <div class="block_item">
                <p>Bank Account：&nbsp;</p>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-2">
                    <p><strong>Quote Notes：</strong></p>
                </div>
                <div class="col-xs-12 col-sm-10">
                    <div class="row">
                        <p>This quote must be approved through Approval Flow due to below issues:</p>
                        <ul>
                            <li>Below GP</li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-2">
                    <p><strong>Reason：</strong></p>
                </div>
                <div class="col-xs-12 col-sm-10">
                    <p>RWD test for JJ.Lin Created by Frank.Chung</p>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12">
                     <table class="table">
                        <tr>
                            <th class="b">No.</th>
                            <th class="b">Category</th>
                            <th class="b">Part No</th>
                            <th class="b">Description</th>
                            <th class="b">Unit Price</th>
                            <th class="b">Qty.</th>
                            <th class="b">Req. Date</th>
                            <th class="b">Sub Total</th>
                            <th class="b">Margin</th>
                            <th class="b">SPR No</th>
                            <th class="b">Special ITP</th>
                        </tr>
                        <tr>
                            <td data-th="No."><p>100</p></td>
                            <td data-th="Category"><p>&nbsp;</p></td>
                            <td data-th="Part No"><p>IPC-510-BTO</p></td>
                            <td data-th="Description"><p>&nbsp;</p></td>
                            <td data-th="Unit Price"><p>986.97</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>&nbsp;</p></td>
                            <td data-th="Sub Total"><p>€986.97</p></td>
                            <td data-th="Margin"><p>32.30%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>101</p></td>
                            <td data-th="Category"><p>CHASSIS FOR IPC-510 [NOTE: IPC-510BP DOES NOT SUPPORT PICMG1.3!]</p></td>
                            <td data-th="Part No"><p>IPC-510BP-00XBE</p></td>
                            <td data-th="Description"><p>CHASSIS, IPC-510 BP Bare Chassis RoHS Ver.B</p></td>
                            <td data-th="Unit Price"><p>€50.00</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€50.00</p></td>
                            <td data-th="Margin"><p>-20.56%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>102</p></td>
                            <td data-th="Category"><p>PSU ATX FOR IPC-510</p></td>
                            <td data-th="Part No"><p>PS8-500ATX-ZE</p></td>
                            <td data-th="Description"><p>POWER SUPPLY, 80+ Bronze PS/2 SPS 500W ATX (FSP) RoHS</p></td>
                            <td data-th="Unit Price"><p>€112.00</p></td>
                            <td data-th="Qty."><p>&nbsp;</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€112.00</p></td>
                            <td data-th="Margin"><p>21.27%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>103</p></td>
                            <td data-th="Category"><p>RAID CONTROLLER (PCI EXPRESS BUS ONLY!)</p></td>
                            <td data-th="Part No"><p>96RC-SAS-4C-PE-HP</p></td>
                            <td data-th="Description"><p>CPU BOARD, HIGHPOINT RAID 10 CARD SAS 4CH PCIEX4(G)</p></td>
                            <td data-th="Unit Price"><p>€168.53</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€168.53</p></td>
                            <td data-th="Margin"><p>16.00%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>104</p></td>
                            <td data-th="Category"><p>SAS HDD 3.5 INCH (1)</p></td>
                            <td data-th="Part No"><p>96HD300G-SS-SG15K1</p></td>
                            <td data-th="Description"><p>HARD DRIVE, SEAGATE 300G 3.5" SAS 15KRPM 6G 16M(G)</p></td>
                            <td data-th="Unit Price"><p>191.18</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€191.18</p></td>
                            <td data-th="Margin"><p>16.00%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>105</p></td>
                            <td data-th="Category"><p>5,25 INCH SATA OPTICAL DRIVE</p></td>
                            <td data-th="Part No"><p>96DVR-24X-ST-LT-B</p></td>
                            <td data-th="Description"><p>OPTICAL DRIVER, LITEON 24X SATA DVD+/-RW BLACK(G)</p></td>
                            <td data-th="Unit Price"><p>€21.28</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€21.28</p></td>
                            <td data-th="Margin"><p>15.98%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>106</p></td>
                            <td data-th="Category"><p>5,25 INCH SATA OPTICAL DRIVE</p></td>
                            <td data-th="Part No"><p>1700003194</p></td>
                            <td data-th="Description"><p>CABLE/WIRE, M Cable SATA 7P/SATA 7P 60CM C=R 180/180</p></td>
                            <td data-th="Unit Price"><p>€1.58</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€1.58</p></td>
                            <td data-th="Margin"><p>58.23%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>107</p></td>
                            <td data-th="Category"><p>ADD-ON IO CARDS 1 (PCI BUS ONLY!)</p></td>
                            <td data-th="Part No"><p>PCI-1612A-CE</p></td>
                            <td data-th="Description"><p>CIRCUIT BOARD, 4-port RS-232/422/485 PCI Comm. Card</p></td>
                            <td data-th="Unit Price"><p>€171.00</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€171.00</p></td>
                            <td data-th="Margin"><p>57.12%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>108</p></td>
                            <td data-th="Category"><p>GRAPHICS CARDS (ADD-ON)</p></td>
                            <td data-th="Part No"><p>96VG-512M-PE-EV6</p></td>
                            <td data-th="Description"><p>CPU BOARD, EVGA NVIDIA210 512M PCIE VGA+DVI+HDMI(G)</p></td>
                            <td data-th="Unit Price"><p>€44.93</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€44.93</p></td>
                            <td data-th="Margin"><p>16.00%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>109</p></td>
                            <td data-th="Category"><p>NETWORK CARD 1 (PCI BUS)</p></td>
                            <td data-th="Part No"><p>96NIC-1G-P-IN4</p></td>
                            <td data-th="Description"><p>CPU BOARD, INTEL NIC 10/100/1000M PCI(G)</p></td>
                            <td data-th="Unit Price"><p>€35.10</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€35.10</p></td>
                            <td data-th="Margin"><p>16.01%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>110</p></td>
                            <td data-th="Category"><p>KEYBOARD</p></td>
                            <td data-th="Part No"><p>KBD-6307</p></td>
                            <td data-th="Description"><p>KEYBOARD, Compact Keyboard 105K with touchpad, English</p></td>
                            <td data-th="Unit Price"><p>€118.00</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€118.00</p></td>
                            <td data-th="Margin"><p>28.84%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>111</p></td>
                            <td data-th="Category"><p>MOUSE</p></td>
                            <td data-th="Part No"><p>96MS-OP2-USB-LT2</p></td>
                            <td data-th="Description"><p>INPUT DEVICE, LOGITECH M100 MOUSE OPTICAL USB(G)</p></td>
                            <td data-th="Unit Price"><p>€13.37</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€13.37</p></td>
                            <td data-th="Margin"><p>16.01%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                        <tr>
                            <td data-th="No."><p>112</p></td>
                            <td data-th="Category"><p>STD ASSEMBLY,FUNCTIONAL TESTING, SW INST (GENERAL)</p></td>
                            <td data-th="Part No"><p>AGS-CTOS-SYS-B</p></td>
                            <td data-th="Description"><p>Standard Assembly + Functional Testing + Softwar</p></td>
                            <td data-th="Unit Price"><p>€60.00</p></td>
                            <td data-th="Qty."><p>1</p></td>
                            <td data-th="Req. Date"><p>1/22/2016</p></td>
                            <td data-th="Sub Total"><p>€60.00</p></td>
                            <td data-th="Margin"><p>0.00%</p></td>
                            <td data-th="SPR No"><p>&nbsp;</p></td>
                            <td data-th="Special ITP"><p>&nbsp;</p></td>
                        </tr>
                     </table>
                 </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 text-right">
                    <p class="block"><strong>Total:</strong>€<span id="lbtotal">986.97</span></p>
                    <p class="block"><span id="spMargin"><strong>Total Margin:</strong><span id="lbTotalMargin">32.30</span>%</span></p>
                    <p class="font-size-8"><span id="spMargin1">Total margin calculated without AGS &amp; PTD items.</span></p>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-offset-4 col-sm-4 col-md-offset-5 col-md-2 margin-bottom-20">
                    <button type="submit" class="btn btn-primary btn-block btn-lg" name="ctl00$ContentPlaceHolder1$btnProcess" id="ContentPlaceHolder1_btnProcess" >Process</button>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
