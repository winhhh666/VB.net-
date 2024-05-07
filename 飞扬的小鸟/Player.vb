Public Class Player
    Public Shared x, y As Single '小鸟的坐标'
    Public Shared width, height As Single '小鸟的尺寸'
    Public Shared life As Boolean = True '小鸟是否死亡'
    Public drop_speed As Single = 2.0F '小鸟当前下落速度'



    Private imgList As List(Of Image) = New List(Of Image) '小鸟图片集合'
    Private index As Integer = 0
    Private time As Integer = 0 '控制小鸟翅膀煽动速度'
    Public Sub New(x As Single, y As Single)
        For i = 0 To 7
            imgList.Add(Image.FromFile("img/" & i & ".png"))
        Next

        Me.x = x : Me.y = y '设置小鸟的坐标'
        width = imgList(0).Width * 0.72F : height = imgList(0).Height * 0.72F

    End Sub

    Public Function draw(e As PaintEventArgs)
        e.Graphics.DrawImage(imgList(index), x, y, width, height)

    End Function

    Public Function run() '小鸟飞翔动作循环改变'
        time += 1
        If time > 6 Then
            time = 0
            index += 1
            If index > 7 Then
                index = 0
            End If

        End If

        '让小鸟下落'
        If drop_speed < 8 Then '下落速度小于8的时候, 增加重力效果'
            drop_speed += Form1.zhongli


        End If
        y += drop_speed
    End Function
End Class
