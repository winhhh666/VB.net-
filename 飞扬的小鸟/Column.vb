Public Class Column
    Public x, y As Single
    '尺寸
    Public width, height As Single
    '尺寸

    Private zhuzi_shang As Image = Image.FromFile("img/column上.png")
    Private zhuzi_xia As Image = Image.FromFile("img/column下.png")
    Private jiange As Single = "240"
    '两个柱子上下之间的间隔
    Public Sub New(x As Single)
        Me.x = x : Me.y = (-200 - -470 + 1) * Rnd() + -470
        'y坐标产生随机值
        width = zhuzi_shang.Width * 0.99F : height = zhuzi_shang.Height * 0.99F
    End Sub

    Public Function draw(e As PaintEventArgs)
        e.Graphics.DrawImage(zhuzi_shang, x, y, width, height)
        e.Graphics.DrawImage(zhuzi_xia, x, y + height + jiange, width, height)
    End Function

    Public Function run(wudi As Boolean)
        x -= Form1.move_speed
        '重新让柱子出现在玩家前面
        If x + height < 432 Then
            x = 432
            Me.y = (-200 - -470 + 1) * Rnd() + -470
        End If

        If Not wudi Then
            '判断玩家是否撞到柱子
            If Player.x + Player.width > x And Player.x < x + width And Not (Player.y > y + height And Player.y + Player.height < y + height + jiange) And Player.life Then
                Player.life = False
                MsgBox("游戏结束")
            End If
        Else

        End If
    End Function






End Class
