<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Transbank.NET._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <h1>Ejemplos Webpay</h1>

    <table border="0" style="width:70%">
  <tr>
    <td><h3>Transacci&oacute;n Normal</h3></td>
    <td><h3><a href="tbk-normal.aspx">webpay Normal</a></h3></td> 
    <td><h3><a href="tbk-nullify-normal.aspx">webpay Normal Anulaci&oacute;n </a></h3></td>
  </tr>

</table>

                    <div id="tab-marca-y-modelo" class="tab-pane active fade in" role="tabpanel">
                        <FORM name="form1" METHOD="POST" ACTION="tbk-normal.aspx?i=0">

							<div class="row">
								<div class="col-sm-12 form-row">
                                    <div class="form-group">
										<input name="txt_nombre" type="text" id="txt_nombre" placeholder="Su nombre" class="form-control">
                                    </div>
                                </div>
                            </div>
							
							<div class="row">
								<div class="col-sm-9 col-xs-9 form-row">
									<div class="form-group">
										<input name="txt_rut" type="text" id="txt_rut" size="12" maxlength="8" placeholder="RUT" class="form-control">
									</div>
								</div>
	   
								<div class="col-sm-1 col-xs-1 form-row">
									<div class="form-group">
										<p>-</p>
									</div>
								</div>
								<div class="col-sm-2 col-xs-2 form-row">
									<div class="form-group">
										<input type="text" name="txt_digito" id="txt_digito" size="2" maxlength="1" class="form-control">
									</div>
								</div>
							</div>
							
							<div class="row">
								<div class="col-sm-12 form-row">
                                    <div class="form-group">
										<select name="cmb_motivo" id="cmb_motivo" class="dropdown form-control">
										   <option value="Pago 1%" selected>Pago 1%</option>
										   <option value="Pago cuota mensual automotora">Pago cuota mensual automotora</option>
										   <option value="Destacar aviso particular">Destacar aviso particular</option>
										   <option value="Tomar fotografias a domicilio">Tomar fotografias a domicilio</option>
										</select>
                                    </div>
                                </div>
							</div>
							
							<div class="row">
								<div class="col-sm-12">
									<div class="form-group">
										<textarea name="txt_comentario" type="text" id="txt_comentario" class="form-control" placeholder="Comentarios (Si es particular, coloque aqui el codigo del auto, patente o marca y modelo del vehiculo)" rows="4"></textarea>
									</div>
								</div>
							</div>
							
							<div class="row">
                                <div class="col-sm-12 form-row">
                                    <div class="form-group">
										<nobr><INPUT TYPE="TEXT" ID="TBK_MONTO2" NAME="TBK_MONTO2" VALUE="" maxlength="7" class="form-control" placeholder="Monto a pagar a Chileautos $ (en pesos" ></nobr> 
									</div>
                                </div>
                            </div>
							
							<div class="row">
                                <div class="col-sm-12 form-row">
                                    <div class="form-group">
										<INPUT TYPE="TEXT" class="form-control" placeholder="N� de orden:" ID="TBK_ORDEN_COMPRA" NAME="TBK_ORDEN_COMPRA" VALUE="oc_123456" >	    
										<INPUT TYPE="HIDDEN" class="form-control" NAME="TBK_ID_SESION" VALUE="">
									    <input type="HIDDEN" class="form-control" name="TBK_TIPO_TRANSACCION" value="TR_NORMAL">
									    <input type="HIDDEN" class="form-control" id="TBK_MONTO" name="TBK_MONTO" value="10000" maxlength="7">
                                    </div>
                                </div>
                            </div>
							
							<div class="row">
                                <div class="col-sm-12 form-row">
                                    <div class="form-group">
										<input name="textfield" type="text" placeholder="Pais pago: Chile" class="form-control" value="Chile" readonly>
                                    </div>
                                </div>
                            </div>
							
							<div class="row">
                                <div class="col-sm-12 form-row">
                                    <div class="form-group">
										<INPUT TYPE="HIDDEN" NAME="TBK_URL_EXITO" class="form-control" VALUE="http://localhost:54128/tbk-normal.aspx"> <BR>
										<INPUT TYPE="HIDDEN" NAME="TBK_URL_FRACASO" class="form-control" VALUE="http://localhost:54128/tbk-normal.aspx"> <BR>
                                    </div>
                                </div>
                            </div>
							
							<div class="row">
                                <div class="col-sm-12 form-row">
                                    <div class="form-group">
										<INPUT TYPE="submit" VALUE="PAGAR CON REDCOMPRA o TARJETA DE CR�DITO" SIZE="20" class="form-control" style="color:#ffffff;background-color:#003366;"> 
                                    </div>
                                </div>
                            </div>
							
							<div id="var_paso" style="visibility:hidden; height:1px; overflow:hidden"></div>

                        </form>
                    </div>

</body>
</html>


