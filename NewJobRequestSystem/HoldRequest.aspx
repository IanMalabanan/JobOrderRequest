﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HoldRequest.aspx.cs" Inherits="NewJobRequestSystem.HoldRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <meta charset="UTF-8" />

    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />

    <meta http-equiv="X-UA-Compatible" content="chrome=1,IE=edge" />

    <meta http-equiv="Content-Language" content="en" />

    <meta name="msapplication-TileColor" content="#2d89ef" />

    <meta name="theme-color" content="#4188c9" />

    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />

    <meta name="apple-mobile-web-app-capable" content="yes" />

    <meta name="mobile-web-app-capable" content="yes" />

    <meta name="HandheldFriendly" content="True" />

    <meta name="MobileOptimized" content="320" />

    <link rel="icon" href="../images/sohbiicon.ico" type="image/x-icon" />

    <link rel="shortcut icon" type="image/x-icon" href="Resources/sohbiicon.ico" />

    <link rel="stylesheet" href="../Bootstrap/Pages/assets/css/font-awesome.min.css" />

    <link rel="stylesheet" href="../Bootstrap/Pages/assets/css/css.css" />

    <link href="../Bootstrap/mycss.css" rel="stylesheet" />

    <script src="../Bootstrap/Pages/assets/js/require.min.js"></script>

    <script>
        requirejs.config({
            baseUrl: '.'
        });
    </script>

    <!-- Dashboard Core -->
    <link href="../Bootstrap/Pages/assets/css/dashboard.css" rel="stylesheet" />

    <script src="../Bootstrap/Pages/assets/js/dashboard.js"></script>

    <!-- c3.js Charts Plugin -->
    <link href="../Bootstrap/Pages/assets/plugins/charts-c3/plugin.css" rel="stylesheet" />

    <script src="../Bootstrap/Pages/assets/plugins/charts-c3/plugin.js"></script>

    <!-- Google Maps Plugin -->
    <link href="../Bootstrap/Pages/assets/plugins/maps-google/plugin.css" rel="stylesheet" />

    <script src="../Bootstrap/Pages/assets/plugins/maps-google/plugin.js"></script>

    <!-- Input Mask Plugin -->
    <script src="../Bootstrap/Pages/assets/plugins/input-mask/plugin.js"></script>

    <style>
        .newfont {
            font-variant: small-caps;
            font-weight: bold;
            letter-spacing: 2.1px;
        }

        /** Columns */
        .rcbHeader ul,
        .rcbFooter ul,
        .rcbItem ul,
        .rcbHovered ul,
        .rcbDisabled ul {
            margin: 0;
            padding: 0;
            width: 100%;
            display: inline-block;
            list-style-type: none;
        }

        .exampleRadComboBox.RadComboBoxDropDown .rcbHeader {
            padding: 5px 27px 4px 7px;
        }

        .rcbScroll {
            overflow: scroll !important;
            overflow-x: hidden !important;
        }

        .col2 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 70%;
            line-height: 14px;
            float: left;
        }

        .col2_3 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 60%;
            line-height: 14px;
            float: left;
        }

        .col2_2 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 35%;
            line-height: 14px;
            float: left;
        }

        .col3 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 35%;
            line-height: 14px;
            float: left;
        }

        .col1 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 30%;
            line-height: 14px;
            float: left;
        }

        .col1_2 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 40%;
            line-height: 14px;
            float: left;
        }

        .example-modal .modal {
            position: relative;
            top: auto;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
        }

        .example-modal .modal {
            background: transparent !important;
        }

        @media (min-width: 992px) {
            .steps {
                padding: 0px;
                background: transparent;
                list-style: none;
                overflow: hidden;
                margin-top: 20px;
                margin-bottom: 20px;
                border-radius: 4px;
                display: table-row;
            }

                .steps > li {
                    display: table-cell;
                    vertical-align: middle;
                    width: 1%;
                    height: 0;
                }

                    .steps > li + li:before {
                        padding: 0;
                        content: "";
                    }

                .steps li a {
                    color: white;
                    text-decoration: none;
                    padding: 10px 0 10px 35px;
                    position: relative;
                    display: inline-block;
                    width: calc(100% - 10px);
                    background-color: #999;
                    text-align: center;
                    height: 100%;
                }

                .steps li.completed a {
                    /*background: #3c763d;*/
                    background: #00a65a;
                }

                    .steps li.completed a:after {
                        /*border-left: 30px solid #3c763d;*/
                        border-left: 30px solid #00a65a;
                    }

                .steps li.active a {
                    background: #3c8dbc;
                }

                    .steps li.active a:after {
                        border-left: 30px solid #3c8dbc;
                    }

                .steps li.rejected_cancelled a {
                    background: #dd4b39;
                }

                    .steps li.rejected_cancelled a:after {
                        border-left: 30px solid #dd4b39;
                    }

                .steps li.pending a {
                    background: #ffa500;
                }

                    .steps li.pending a:after {
                        border-left: 30px solid #ffa500;
                    }



                .steps li:first-child a {
                    padding-left: 15px;
                }

                .steps li:last-of-type a {
                    width: calc(100% - 38px);
                }

                .steps li a:before {
                    content: " ";
                    display: block;
                    width: 0;
                    height: 0;
                    border-top: 50px solid transparent; /* height not equal parent */
                    border-bottom: 50px solid transparent; /* height not equal parent */
                    border-left: 30px solid white;
                    position: absolute;
                    top: 50%;
                    margin-top: -50px; /* height not equal parent */
                    margin-left: 1px;
                    left: 100%;
                    z-index: 1;
                }

                .steps li a:after {
                    content: " ";
                    display: block;
                    width: 0;
                    height: 0;
                    border-top: 50px solid transparent; /* height not equal parent */
                    border-bottom: 50px solid transparent; /* height not equal parent */
                    border-left: 30px solid #999;
                    position: absolute;
                    top: 50%;
                    margin-top: -50px; /* height not equal parent */
                    left: 100%;
                    z-index: 2;
                }
        }

        @media (max-width: 991px) {
            .steps {
                padding: 8px 15px;
                margin-bottom: 20px;
                list-style: none;
                background-color: #f5f5f5;
                border-radius: 4px;
                overflow: auto;
            }

                .steps > li {
                    display: block;
                }

                .steps li a {
                    color: #777;
                }

                .steps > li:before {
                    padding: 0 5px;
                    color: #ccc;
                    content: "\e080";
                    font-family: 'Glyphicons Halflings';
                    font-style: normal;
                    font-weight: 400;
                    line-height: 1;
                    -webkit-font-smoothing: antialiased;
                    -moz-osx-font-smoothing: grayscale;
                }

                .steps li.completed:before {
                    content: "\e013";
                    color: #3c763d;
                    font-family: 'Glyphicons Halflings';
                    font-style: normal;
                    font-weight: 400;
                    line-height: 1;
                    -webkit-font-smoothing: antialiased;
                    -moz-osx-font-smoothing: grayscale;
                }

                .steps li.completed a {
                    color: inherit;
                }

                .steps li.active:before {
                    color: #8a6d3b;
                }

                .steps > .active {
                    color: #999;
                }

                .steps li:first-child a {
                    padding-left: inherit;
                }

                .steps li:last-of-type a {
                    width: inherit;
                }
        }
    </style>

</head>

<body>
    <form id="form1" runat="server">
        <script type="text/javascript">

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

                return oWindow;
            }

            function CloseWindow() {
                GetRadWindow().BrowserWindow.location.reload();
                GetRadWindow().close();
            }

        </script>

        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>


        <div class="row" style="margin-left:2px;margin-right:2px;">

            <div class="col-12">

                <asp:TextBox ID="tbHold" style="margin-top:5px" runat="server" ReadOnly="false" placeholder="Enter Text Here..." CssClass="form-control" Height="180px" TextMode="MultiLine"></asp:TextBox>

            </div>

        </div>

        <br />

        <div class="row" style="margin-left:2px;margin-right:2px">

            <div class="col-12">

                <telerik:RadButton ID="btnHold" runat="server" Style="height: 35px;"
                    Text="Hold" SingleClick="true" Width="100px" Primary="true" RenderMode="Lightweight"
                    OnClientClick="return confirm('This process may reject the request. Do you want to proceed?')"
                    SingleClickText="Processing..." CssClass="pull-right" OnClick="btnReject_OnClick">
                </telerik:RadButton>

            </div>

        </div>

    </form>
</body>
</html>

