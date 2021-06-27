/*---------------------------------------------*/
/* 作成日　2021/5/30(日)～6/3(火)
-----------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OthelloApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			CreatePictureBoxes();

			GameStart();
		}

		Point leftTopPoint = new Point(30, 30);
		Stone[,] StonePosition = new Stone[8, 8];


		/// <summary>
		/// 初期設定
		/// </summary>
		void CreatePictureBoxes()
		{
			for(int row=0; row<8; row++)
			{
				for(int colum = 0; colum < 8; colum++)
				{
					Stone stone = new Stone(colum, row);
					stone.Parent = this;
					stone.Size = new Size(40, 40);
					stone.BorderStyle = BorderStyle.FixedSingle;
					stone.Location = new Point(leftTopPoint.X +colum * 40, leftTopPoint.Y + row * 40);
					StonePosition[colum, row] = stone;
					stone.StoneClick += Box_PictureBoxExClick;
					stone.BackColor = Color.Green;
				}
			}
		}


		/// <summary>
		/// 裏返し可能判定(上)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>裏返し可能な石(上)</returns>
		List<Stone> GetReverseOnPutUp(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor = StoneColor.None;
			if(color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if(y - 1 < 0)
				return stones;

			var s = StonePosition[x, y - 1];
			if(s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for(int i=0; ; i++)
			{
				if(y - 1 - i < 0)
					return new List<Stone>();

				var s1 = StonePosition[x, y - 1 - i];
				if(s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if(s1.StoneColor == color)
					return stones;
				if(s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		/// <summary>
		/// 裏返し可能判定(下)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>裏返し可能な石(下)</returns>
		List<Stone> GetReverseOnPutDown(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor = StoneColor.None;
			if(color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if(y + 1 > 7)
				return stones;

			var s = StonePosition[x, y + 1];
			if(s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for(int i = 0; ; i++)
			{
				if(y + 1 + i > 7)
					return new List<Stone>();

				var s1 = StonePosition[x, y + 1 + i];
				if(s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if(s1.StoneColor == color)
					return stones;
				if(s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		/// <summary>
		/// 裏返し可能判定(左)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>裏返し可能な石(左)</returns>
		List<Stone> GetReverseOnPutLeft(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor;
			if(color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if(x - 1 < 0)
				return stones;

			var s = StonePosition[x-1, y];
			if(s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for(int i = 0; ; i++)
			{
				if(x - 1 - i < 0)
					return new List<Stone>();

				var s1 = StonePosition[x-1-i, y];
				if(s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if(s1.StoneColor == color)
					return stones;
				if(s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		/// <summary>
		/// 裏返し可能判定(右)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>裏返し可能な石(右)</returns>
		List<Stone> GetReverseOnPutRight(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor;
			if(color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if(x + 1 > 7)
				return stones;

			var s = StonePosition[x + 1, y];
			if(s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for(int i = 0; ; i++)
			{
				if(x + 1 + i > 7)
					return new List<Stone>();

				var s1 = StonePosition[x + 1 + i, y];
				if(s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if(s1.StoneColor == color)
					return stones;
				if(s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		/// <summary>
		/// コンピュータの裏返し可能判定(上)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>コンピュータの裏返し可能な石(上)</returns>
		List<Stone> GetReverseOnPutLeftDown(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor;
			if (color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if (x - 1 < 0)
				return stones;
			if (y + 1 > 7)
				return stones;

			var s = StonePosition[x - 1, y + 1];
			if (s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for (int i = 0; ; i++)
			{
				if (x - 1 - i < 0)
					return new List<Stone>();

				if (y + 1 + i > 7)
					return new List<Stone>();

				var s1 = StonePosition[x - 1 - i, y + 1 + i];
				if (s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if (s1.StoneColor == color)
					return stones;

				if (s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		/// <summary>
		/// コンピュータの裏返し可能判定(下)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>コンピュータの裏返し可能な石(下)</returns>
		List<Stone> GetReverseOnPutRightDown(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor;
			if (color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if (x + 1 > 7)
				return stones;
			if (y + 1 > 7)
				return stones;

			var s = StonePosition[x + 1, y + 1];
			if (s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for (int i = 0; ; i++)
			{
				if (x + 1 + i > 7)
					return new List<Stone>();

				if (y + 1 + i > 7)
					return new List<Stone>();

				var s1 = StonePosition[x + 1 + i, y + 1 + i];
				if (s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if (s1.StoneColor == color)
					return stones;

				if (s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		/// <summary>
		/// コンピュータの裏返し可能判定(左)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>コンピュータの裏返し可能な石(左)</returns>
		List<Stone> GetReverseOnPutLeftTop(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor;
			if(color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if(x - 1 < 0)
				return stones;
			if(y - 1 < 0)
				return stones;

			var s = StonePosition[x - 1, y-1];
			if(s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for(int i = 0; ; i++)
			{
				if(x - 1 - i < 0)
					return new List<Stone>();

				if(y - 1 - i < 0)
					return new List<Stone>();

				var s1 = StonePosition[x - 1 - i, y - 1 - i];
				if(s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if(s1.StoneColor == color)
					return stones;

				if(s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		/// <summary>
		/// コンピュータの裏返し可能判定(右)
		/// </summary>
		/// <param name="x">石の横側</param>
		/// <param name="y">石の縦側</param>
		/// <param name="color">石の色</param>
		/// <returns>コンピュータの裏返し可能な石(右)</returns>
		List<Stone> GetReverseOnPutRightTop(int x, int y, StoneColor color)
		{
			List<Stone> stones = new List<Stone>();
			StoneColor enemyColor;
			if(color == StoneColor.Black)
				enemyColor = StoneColor.White;
			else
				enemyColor = StoneColor.Black;

			if(x + 1 > 7)
				return stones;
			if(y - 1 < 0)
				return stones;

			var s = StonePosition[x + 1, y - 1];
			if(s.StoneColor == color || s.StoneColor == StoneColor.None)
				return stones;

			stones.Add(s);

			for(int i = 0; ; i++)
			{
				if(x + 1 + i > 7)
					return new List<Stone>();

				if(y - 1 - i < 0)
					return new List<Stone>();

				var s1 = StonePosition[x + 1 + i, y - 1 - i];
				if(s1.StoneColor == enemyColor)
				{
					stones.Add(s1);
					continue;
				}
				if(s1.StoneColor == color)
					return stones;

				if(s1.StoneColor == StoneColor.None)
					return new List<Stone>();
			}
		}

		
		/// <summary>
		///　コンピュータの思考
		/// </summary>
		void EnemyThink()
		{
			bool isComPassed = false;
			bool isYouPassed = false;
			while(true)
			{
				// Cast メソッドで 1次元に
				var stones = StonePosition.Cast<Stone>();

				// 石が置かれていない場所で挟むことができる場所を探す。
				stones = stones.Where(xx => xx.StoneColor == StoneColor.None && GetRevarseStones(xx.Colum, xx.Row, StoneColor.White).Any());
				var hands = stones.ToList();
				int count = hands.Count();
				if(count > 0)
				{
					// 石をおける場所が見つかったらそのなかから適当に次の手を選ぶ
					Stone stone = hands[random.Next() % count];

					// 石を置いてひっくり返す
					StonePosition[stone.Colum, stone.Row].StoneColor = StoneColor.White;
					List<Stone> stones1 = GetRevarseStones(stone.Colum, stone.Row, StoneColor.White);
					stones1.Select(xx => xx.StoneColor = StoneColor.White).ToList();
				}
				else
				{
					if(isYouPassed)
					{
						// 双方に「手」が存在しない場合はゲームセットとする
						OnGameset();
						return;
					}

					// 石をおける場所が見つからない場合はパス
					isComPassed = true;
				}

				// プレイヤーの手番だが、「手」は存在するのか？
				stones = StonePosition.Cast<Stone>();
				stones = stones.Where(xx => xx.StoneColor == StoneColor.None && GetRevarseStones(xx.Colum, xx.Row, StoneColor.Black).Any());
				hands = stones.ToList();
				count = hands.Count();

				// 「手」が存在するならプレーヤーの手番とする
				if(count > 0)
				{
					isYour = true;
					if(isComPassed)
						toolStripStatusLabel1.Text = "コンピュータはパスしました。あなたの手番です。";
					else
						toolStripStatusLabel1.Text = "あなたの手番です。";
					return;
				}
				else
				{
					// 「手」が存在しない場合はもう一度コンピュータの手番とする
					if(!isComPassed)
					{
						isYouPassed = true;
						toolStripStatusLabel1.Text = "あなたの手番ですが手がありません";
					}
					else
					{
						// 双方に「手」が存在しない場合はゲームセットとする
						OnGameset();
						return;
					}
				}
			}
		}


		/// <summary>
		/// ゲームスタート
		/// </summary>
		void GameStart()
		{
			var stones = StonePosition.Cast<Stone>();
			foreach(Stone stone in stones)
				stone.StoneColor = StoneColor.None;

			StonePosition[3, 3].StoneColor = StoneColor.Black;
			StonePosition[4, 4].StoneColor = StoneColor.Black;

			StonePosition[3, 4].StoneColor = StoneColor.White;
			StonePosition[4, 3].StoneColor = StoneColor.White;

			isYour = true;
			toolStripStatusLabel1.Text = "あなたの手番です。";
			isEnd = false;
		}


		/// <summary>
		/// ゲームセット
		/// </summary>
		void OnGameset()
		{
			var stones = StonePosition.Cast<Stone>();

			int blackCount = stones.Count(xx => xx.StoneColor == StoneColor.Black);
			int whiteCount = stones.Count(xx => xx.StoneColor == StoneColor.White);

			string str = "";
			if(blackCount != whiteCount)
			{
				string winner = blackCount > whiteCount ? "黒" : "白";
				str = String.Format("終局しました。{0} 対 {1} で {2} の勝ちです。", blackCount, whiteCount, winner);
			}
			else
			{
				str = String.Format("終局しました。{0} 対 {1} で 引き分けです。", blackCount, whiteCount);
			}
			toolStripStatusLabel1.Text = str;
			isEnd = true;
			return;
		}

		bool isYour = true;
		bool isEnd = false;
		private async void Box_PictureBoxExClick(int x, int y)
		{
			if (isEnd)
				return;

			// 自分の手番か確認する
			if(!isYour)
				return;


			// 着手可能な場所か調べる
			List<Stone> stones = GetRevarseStones(x, y, StoneColor.Black);

			if (stones.Count != 0)
			{
				StonePosition[x, y].StoneColor = StoneColor.Black;
				stones.Select(xx => xx.StoneColor = StoneColor.Black).ToList();
				isYour = false;

				toolStripStatusLabel1.Text = "コンピュータが考えています";

				await Task.Delay(1000);

				EnemyThink();
			}
			else
				toolStripStatusLabel1.Text = "ここには打てません";
		}

		Random random = new Random();


		/// <summary>
		/// クリック時に石が置けるかを判定(全8方向)
		/// </summary>
		/// <param name="x">横</param>
		/// <param name="y">縦</param>
		/// <param name="stoneColor">石の色</param>
		/// <returns>石が置ける位置</returns>
		List<Stone> GetRevarseStones(int x, int y, StoneColor stoneColor)
		{
			List<Stone> stones = new List<Stone>();
			stones.AddRange(GetReverseOnPutUp(x, y, stoneColor));  // 上方向に挟めているものを取
			stones.AddRange(GetReverseOnPutDown(x, y, stoneColor)); // 下
			stones.AddRange(GetReverseOnPutLeft(x, y, stoneColor)); // 左
			stones.AddRange(GetReverseOnPutRight(x, y, stoneColor)); // 右
			stones.AddRange(GetReverseOnPutLeftTop(x, y, stoneColor)); // 左上
			stones.AddRange(GetReverseOnPutLeftDown(x, y, stoneColor));	// 左下
			stones.AddRange(GetReverseOnPutRightTop(x, y, stoneColor)); // 右上
			stones.AddRange(GetReverseOnPutRightDown(x, y, stoneColor)); // 右下

			return stones;
		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GameStart();
		}
	}


	/// <summary>
	/// 石の設定
	/// </summary>
	public class Stone : PictureBox
	{
		StoneColor stoneColor = StoneColor.None;
		public int Colum
		{
			protected set;
			get;
		} = 0;
		public int Row
		{
			protected set;
			get;
		} = 0;

		public Stone(int colum, int row)
		{
			Colum = colum;
			Row = row;

			Click += Stone_Click;
		}

		public delegate void StoneClickHandler(int x, int y);
		public event StoneClickHandler StoneClick;
		private void Stone_Click(object sender, EventArgs e)
		{
			StoneClick?.Invoke(Colum, Row);
		}


		/// <summary>
		/// 石の色の設定
		/// </summary>
		public StoneColor StoneColor
		{
			get { return stoneColor; }
			set
			{
				SizeMode = PictureBoxSizeMode.StretchImage;

				stoneColor = value;
				if(value == StoneColor.Black)
					Image = OthelloApp1.Properties.Resources.black;
				if(value == StoneColor.White)
					Image = OthelloApp1.Properties.Resources.white;
				if(value == StoneColor.None)
					Image = null;

				return;
				if(value == StoneColor.Black)
					BackColor = Color.Black;
				if(value == StoneColor.White)
					BackColor = Color.White;
				if(value == StoneColor.None)
					BackColor = Color.Green;

			}
		}
	}

	public enum StoneColor 
	{
		None = 0,
		Black = 1,
		White = 2,
	}
}
