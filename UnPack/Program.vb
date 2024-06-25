Imports System
Imports System.Text
Imports System.IO
Imports System.IO.Compression


Module Program

    Private br As BinaryReader
    Private des As String
    Private source As String
    Private buffer As Byte()
    Private subfiles As New List(Of FileData)()



    Sub Main(args As String())

        If args.Count = 0 Then
            Console.WriteLine("UnPack Tool - 2CongLc.vn")
        Else
            source = args(0)
        End If

        If File.Exists(source) Then

            ' Lấy thông tin đường dẫn trích xuất tệp
            des = Path.GetDirectoryName(source) & "\" & Path.GetFileNameWithoutExtension(source) & "\"
            br = New BinaryReader(File.OpenRead(source))

            Dim sign As String = New String(br.ReadChars(3))
            Dim vers As String = br.ReadInt32


            br.BaseStream.Position = 19

            ' Lưu dữ liệu Block
            For i As Int32 = 0 To 469
                subfiles.Add(New FileData)
            Next

            Directory.CreateDirectory(des)

            ' Trích xuất dữ liệu Block
            For Each fd As FileData In subfiles

                Console.WriteLine("File Offset : {0} - File Size : {1} - File Name : {2}", fd.offset, fd.size, fd.name)
                br.BaseStream.Position = fd.offset
                Dim buffer As Byte() = br.ReadBytes(fd.size)

                Using bw As BinaryWriter = New BinaryWriter(File.Create(des & fd.name))
                    bw.Write(buffer)
                End Using

            Next
            Console.WriteLine("Unpack Done !")

        End If
        Console.ReadLine()
    End Sub

    ' Cấu trúc dữ liệu Block
    Class FileData
        Public name As String = New String(br.ReadChars(259)).TrimEnd(ChrW(0))
        Public offset As Int32 = br.ReadInt32
        Public size As Int32 = br.ReadInt32
    End Class
End Module
