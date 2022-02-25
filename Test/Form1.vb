Public Class Form1
    Public ioMgr As Ivi.Visa.Interop.ResourceManager
    Public instrument As Ivi.Visa.Interop.FormattedIO488
    Public ERRO_ENDEREÇO, End_EUT As Integer
    Public end1 As String
    'Private WithEvents tim As New DataGridView

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As _
     System.EventArgs) Handles Button1.Click

        Dim idn, TEXTO_GPIB, texto2, End_EUT_TXT, ERRO_ENDEREÇO As String
        Dim resposta_gpib, CONTADOR_ERRO, CONTA_VOLTA1, i As Integer
        Dim nametxt As String = "Padrao_txt" & i.ToString

        ioMgr = New Ivi.Visa.Interop.ResourceManager
        instrument = New Ivi.Visa.Interop.FormattedIO488
VOLTA1:
        End_EUT_TXT = ""
        TEXTO_GPIB = "O ENDEREÇO FORNECIDO PARA O MEDIDOR DE FREQUÊNCIA PADRÃO SERÁ GPIB?" & Chr(13) & Chr(13) & "SIM - ENDEREÇO GPIB" & Chr(13) & "NÃO - ENDEREÇO LAN (REDE)"
        resposta_gpib = MsgBox(TEXTO_GPIB, vbYesNo, "TIPO DE ENDEREÇO")
        If resposta_gpib = 6 Then
            texto2 = "Entre com o Endereço GPIB: "
            End_EUT = InputBox(texto2, "ENDEREÇO")
            If IsNumeric(End_EUT) = False Then
                If CONTADOR_ERRO >= 2 Then
                    MsgBox("TESTE ENCERRADO", vbOKOnly, "FIM")
                End If
                MsgBox("VALOR INCORRETO", vbOKOnly, "ENDEREÇO GPIB")
                CONTADOR_ERRO = CONTADOR_ERRO + 1
                GoTo FIM2
            Else
                If End_EUT = 0 Then
                    MsgBox("ENDEREÇO GPIB INCORRETO, IGUAL À OUTRO JÁ ESCOLHIDO OU O EQUIPAMENTO NÃO FOI CONECTADO AO CABO GPIB.", vbOKOnly, "ERRO")
                    CONTA_VOLTA1 = CONTA_VOLTA1 + 1
                    If CONTA_VOLTA1 = 5 Then
                        MsgBox("TESTE ENCERRADO", vbOKOnly, "FIM")
                        GoTo FIM2
                    Else
                        GoTo VOLTA1
                    End If

                End If
            End If
            End_EUT_TXT = "GPIB0::" & End_EUT & "::INSTR"
        Else
            texto2 = "Entre com o Endereço LAN do GERADOR: "
            End_EUT_TXT = InputBox(texto2, "ENDEREÇO")
            End_EUT_TXT = "TCPIP0::" & End_EUT_TXT & "::INSTR"
        End If

        'Call endereco(End_EUT_TXT)

        If ERRO_ENDEREÇO = 1 Then
            TEXTO_GPIB = "HOUVE UM ERRO" & Chr(13) & "VERIFIQUE O SETUP E EXECUTE O SOFTWARE NOVAMENTE"
            MsgBox(TEXTO_GPIB, vbOKOnly, "ERRO DE SETUP")
            GoTo FIM2
        End If

        instrument.IO = ioMgr.Open(End_EUT_TXT)
        instrument.WriteString("*IDN?")
        idn = instrument.ReadString()

        Dim palavras As String() = idn.Split(",")
        TextBoxFab.Text = palavras(0)
        TextBoxMod.Text = palavras(1)
        TextBoxNS.Text = palavras(2)


FIM2:

    End Sub

    Private Sub endereco(End_EUT_TXT)
        Dim strReturn As String
        Dim intDevice As Integer

        On Error GoTo fim
        ioMgr = New Ivi.Visa.Interop.ResourceManager
        instrument = New Ivi.Visa.Interop.FormattedIO488

        instrument.IO = ioMgr.Open(End_EUT_TXT)
        instrument.WriteString("*IDN?")

        ERRO_ENDEREÇO = 0
        Exit Sub
        instrument.IO.Close()
fim:
        ERRO_ENDEREÇO = 1
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form1.ActiveForm.Close()

    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBoxNS.TextChanged

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBoxMod_TextChanged(sender As Object, e As EventArgs) Handles TextBoxMod.TextChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim PONTO, LINHA, CELL, num As Integer
        Dim avg, L As Double
        Dim med, End_EUT_TXT As String
        Dim date1 As DateTime = #02/25/2022 03:00PM#


        'With DataGridView1
        'SelectionMode = DataGridViewSelectionMode.FullRowSelect
        '.Columns.Add()

        ' End With

        End_EUT_TXT = "GPIB0::" & End_EUT & "::INSTR"
        instrument.IO = ioMgr.Open(End_EUT_TXT)

        instrument.WriteString("*RST")
        instrument.WriteString("INP1:COUP DC")
        instrument.WriteString("INP1:IMP 50")
        instrument.WriteString("INIT")
        LINHA = 0
        CELL = 0
        Do
            PONTO = PONTO + 1

            System.Threading.Thread.Sleep(1000)
            instrument.WriteString("READ?")
            med = instrument.ReadString()

            'Me.DataGridView1.Rows(LINHA).Cells(CELL).Value = date1
            Me.DataGridView1.Rows.Add(date1, med)
            'LINHA = LINHA + 1
            'CELL = CELL + 1
            'Me.DataGridView1.Rows(LINHA).Cells(CELL).Value = med
            'Me.DataGridView1.Rows.Add(med)


            Me.DataGridView1.AutoResizeColumns()

        Loop Until PONTO >= 5
        instrument.IO.Close()

        num = DataGridView1.RowCount.ToString - 1
        TextBoxMedidas.Text = num

        For i = 0 To num - 1
            avg = avg + DataGridView1.Rows(i).Cells(1).Value
        Next

        TextBoxMedia.Text = avg / num
        L = DataGridView1.Rows(1).Cells(1).Value
        TextBoxDesvio.Text = L
        'TextBoxMedia.Text = TextBoxMedia.Text / TextBoxMedidas.Text
        avg = TextBoxMedia.Text
        'TextBoxMedia.Text = med / PONTO


        ' ioMgr = New Ivi.Visa.Interop.ResourceManager
        'instrument = New Ivi.Visa.Interop.FormattedIO488
        ' End_EUT = 5
        'End_EUT_TXT = "GPIB0::" & End_EUT & "::INSTR"
        'instrument.IO = ioMgr.Open(End_EUT_TXT)
        ' instrument.WriteString("*IDN?")
        'idn = instrument.ReadString()
        ' TEXTO_INFORMACAO = idn
        'TextBox1.Text = TEXTO_INFORMACAO
        ' instrument.IO.Close()


    End Sub
End Class
