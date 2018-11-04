using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NeoNftProject.Data;
using NeoNftProject.Models;
using Newtonsoft.Json;

namespace NeoNftProject.Services
{
	public class MainService : IMainService
	{
		private readonly NeoNftContext db;

		public MainService(NeoNftContext db)
		{
			this.db = db;
		}

        public string[] Battle(BattleInputModel model)
        {
            Token firstToken = db.Tokens.FirstOrDefault(c => c.Id == model.Player1);
            Token secondToken = db.Tokens.FirstOrDefault(c => c.Id == model.Player2);
            if (firstToken == null || secondToken == null)
            {
                return new string[0];
            }

            string[] moves = Brawl(firstToken, secondToken).ToArray();

            return moves;
        }

        public List<string> Brawl(Token player1, Token player2)
        {
            Random randP1 = new Random(player1.TxId.GetHashCode());
            Random randP2 = new Random(player2.TxId.GetHashCode());
            List<string> result = new List<string>();

            while (player1.Health > 0 && player2.Health > 0)
            {
                bool CriticalStrikeP1 = randP1.Next(100) > player1.CriticalStrike;
                int damageP1 = (player1.Agility / 7) * (1 + player1.AttackSpeed / 100) * (1 - player2.Versatility / 100) - randP2.Next(player2.Mastery);
                damageP1 = CriticalStrikeP1 ? (int)(damageP1 * 1.5) : damageP1;

                bool CriticalStrikeP2 = randP2.Next(100) > player2.CriticalStrike;
                int damageP2 = (player2.Agility / 7) * (1 + player2.AttackSpeed / 100) * (1 - player1.Versatility / 100) - -(int)randP1.Next(player1.Mastery);
                damageP2 = CriticalStrikeP2 ? (int)(damageP2 * 1.5) : damageP2;

                result.Add("Player1 attacks for " + damageP1);
                result.Add("Player2 atacks for " + damageP2);

                player2.Health -= damageP1;
                result.Add("Player2 Health " + player2.Health);

                //Check if P2 is finished
                if (player2.Health <= 0)
                {
                    result.Add("Player 1 wins.");
                    return result;
                }

                player1.Health -= damageP2;
                result.Add("Player1 Health " + player1.Health);
            }

            result.Add("Player 2 wins. ");
            return result;
        }

        public Token Breed(Token player1, Token player2)
        {
            Random randP1 = new Random(player1.TxId.GetHashCode());
            Random randP2 = new Random(player2.TxId.GetHashCode());

            // New token to add
            Token token = new Token()
            {
                Health = randP1.Next(Math.Min(player1.Health, player2.Health), Math.Max(player1.Health, player2.Health)),
                Stamina = randP1.Next(Math.Min(player1.Stamina, player2.Stamina), Math.Max(player1.Stamina, player2.Stamina)),
                Agility = randP1.Next(Math.Min(player1.Agility, player2.Agility), Math.Max(player1.Agility, player2.Agility)),
                Mana = randP1.Next(Math.Min(player1.Mana, player2.Mana), Math.Max(player1.Mana, player2.Mana)),
                CriticalStrike = randP1.Next(Math.Min(player1.CriticalStrike, player2.CriticalStrike), Math.Max(player1.CriticalStrike, player2.CriticalStrike)),
                AttackSpeed = randP1.Next(Math.Min(player1.AttackSpeed, player2.AttackSpeed), Math.Max(player1.AttackSpeed, player2.AttackSpeed)),
                Mastery = randP1.Next(Math.Min(player1.Mastery, player2.Mastery), Math.Max(player1.Mastery, player2.Mastery)),
                Versatility = randP1.Next(Math.Min(player1.Versatility, player2.Versatility), Math.Max(player1.Versatility, player2.Versatility))
            };
            return token;
        }

        public Token Mutate(Token player, int maximalChange)
        {
            Random randP1 = new Random(player.TxId.GetHashCode());

            int randomIndex = randP1.Next(0, 7);
            switch (randomIndex)
            {
                case 0:
                    player.Health = randP1.Next(player.Health - maximalChange, player.Health + maximalChange);
                    break;
                case 1:
                    player.Stamina = randP1.Next(player.Stamina - maximalChange, player.Stamina + maximalChange);
                    break;
                case 2:
                    player.Agility = randP1.Next(player.Agility - maximalChange, player.Agility + maximalChange);
                    break;
                case 3:
                    player.Mana = randP1.Next(player.Mana - maximalChange, player.Mana + maximalChange);
                    break;
                case 4:
                    player.CriticalStrike = randP1.Next(player.CriticalStrike - maximalChange, player.CriticalStrike + maximalChange);
                    break;
                case 5:
                    player.AttackSpeed = randP1.Next(player.AttackSpeed - maximalChange, player.AttackSpeed + maximalChange);
                    break;
                case 6:
                    player.Mastery = randP1.Next(player.Mastery - maximalChange, player.Mastery + maximalChange);
                    break;
                case 7:
                    player.Versatility = randP1.Next(player.Versatility - maximalChange, player.Versatility + maximalChange);
                    break;
            }
            return player;
        }

        public Token Breed(BreedInputModel model)
        {
            Token firstToken = db.Tokens.FirstOrDefault(c => c.Id == model.Player1);
            Token secondToken = db.Tokens.FirstOrDefault(c => c.Id == model.Player2);

            if (firstToken == null || secondToken == null)
            {
                return null;
            }

            Token newToken = this.Breed(firstToken, secondToken);
            newToken.AddressId = firstToken.AddressId;

            //db.Add(newToken);
            //db.SaveChanges();

            return newToken;
        }

        public int CreateAddress(string address)
		{
			var addressObj = db.Addresses.FirstOrDefault(c => c.AddressName == address);

			if (addressObj == null)
			{
				var newAddress = new Address() { AddressName = address };
				db.Addresses.Add(newAddress);
				db.SaveChanges();

				return newAddress.Id;
			}
			else
			{
				return addressObj.Id;
			}

		}

		public int CreateAuction(Auction auction)
		{
			db.Auctions.Add(auction);
			db.SaveChanges();

			return auction.Id;
		}

		public int CreateAuction(CreateAuctionInputModel auction)
		{
			var span = DateTime.Parse(auction.EndDate).Subtract(DateTime.Parse(auction.StartDate));
			var duration = (long)span.TotalMinutes;

			var newAuction = new Auction() {
						StartDate = DateTime.Parse(auction.StartDate),
						EndDate = DateTime.Parse(auction.EndDate),
						CurrentPrice = auction.StartPrice,
						StartPrice = auction.StartPrice,
						Duration = duration,
						IsActive = 1,
						EndPrice = 0 };

			var token = db.Tokens.FirstOrDefault(c => c.Id == auction.TokenId);

			if (token == null)
			{
				return 0;
			}

			var address = db.Addresses.FirstOrDefault(c => c.Id == auction.AddressId);
			if (address == null)
			{
				return -1;
			}

			newAuction.Token = token;
			newAuction.Address = address;
			db.Add(newAuction);
			db.SaveChanges();

			return newAuction.Id;
		}

		public int CreateToken(CreateTokenInputModel token)
		{
			var newToken = new Token() { Health = token.Health,
							AttackSpeed = token.AttackSpeed,
							Agility = token.Agility, ImageUrl= token.ImageUrl,
							CriticalStrike = token.CriticalStrike,
							Mana = token.Mana,
							Mastery = token.Mastery,
							Nickname = token.Nickname,
							Stamina = token.Stamina,
							Versatility = token.Versatility,
							TxId = token.TxId,
							IsBrawling = token.IsBrawling,
							IsBreeding = token.IsBreeding,
							Level = token.Level,
							Experience = token.Experience };

			var address = db.Addresses.FirstOrDefault(c => c.Id == token.AddressId);

			if (address == null)
			{
				return 0;
			}

			newToken.Address = address;
			db.Add(newToken);
			db.SaveChanges();

			return newToken.Id;
		}

		public int CreateTransaction(CreateTransactionInputModel model)
		{
			var transaction = new Transaction();
			var receiver = db.Addresses.FirstOrDefault(c => c.Id == model.ReceiverId);
			var sender = db.Addresses.FirstOrDefault(c => c.Id == model.SenderId);
			var token = db.Tokens.FirstOrDefault(c => c.Id == model.TokenId);

			if (receiver == null) return 0;
			if (sender == null) return -1;
			if (token == null) return -2;

			token.Address = receiver;
			transaction.Receiver = receiver;
			transaction.Sender = sender;

			db.Add(transaction);
			db.Update(token);
			db.SaveChanges();

			return transaction.Id;

		}

		public ICollection<Auction> GetAuctions()
		{
			return this.db.Auctions
				.Where(c=> c.IsActive == 1)
				.ToList();
		}

		public ICollection<Token> GetOwnedTokens(string owner)
		{
            var tokens = db.Tokens.Where(c => c.Address.AddressName == owner).ToList();

			return tokens;
		}

        public async Task<int> SearchBreedPartner()
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                   { "content", "SilentSlap is looking for a partner to breed. Come and create new generations with him! https://breedandbrawlgame.com/" }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://discordapp.com/api/webhooks/508308351151505419/hdZ29UVDYFQoai7rwnNaAoDioa9YEfqAeTKH7AiitOhPIHIsQ6SH7U-1XD8waKXmgOqJ", content);
            }

            return 0;
        }

        public async Task<int> SearchFightPartner()
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                   { "content", "SilentSlap is looking for a fight. Come and fight him! https://breedandbrawlgame.com/ " }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://discordapp.com/api/webhooks/508270591720161283/9FH2sX0vxmlRj6j3KI1dC3PD3m9nZL3PNkHwygJ56MLfAJkzV7GOb-BrbgSyMPx2O7sC", content);
            }

            return 0;
        }

        public Auction UpdateAuction(UpdateAuctionModel model)
		{
			var auction = db.Auctions.FirstOrDefault(c => c.Id == model.AuctionId);

			if (auction == null) return null;


			auction.CurrentPrice = model.CurrentPrice;

			if (model.IsSold != null)
			{
				auction.IsActive = 0;
				auction.EndPrice = (decimal)model.EndPrice;
			}

			db.Update(auction);
			db.SaveChanges();

			return auction;
		}

		public Token UpdateToken(UpdateTokenInputModel model)
		{
			var token = db.Tokens.FirstOrDefault(c => c.Id == model.Id);
			if (token == null) return null;

			// Manual map
			token.AddressId = (model.AddressId != 0 && model.AddressId != null)?  (int)model.AddressId : token.AddressId;
			token.Nickname = (model.Nickname != string.Empty) ? model.Nickname : token.Nickname;
			token.TxId = (model.TxId != string.Empty) ? model.TxId : token.TxId;
			token.Health = (model.Health != 0 && model.Health != null) ? (int)model.Health : token.Health;
			token.Mana = (model.Mana != 0 && model.Mana != null) ? (int)model.Mana : token.Mana;
			token.Agility = (model.Agility != 0 && model.Agility != null) ? (int)model.Agility : token.Agility;
			token.Stamina = (model.Stamina != 0 && model.Stamina != null) ? (int)model.Stamina : token.Stamina;
			token.CriticalStrike = (model.CriticalStrike != 0 && model.CriticalStrike != null) ? (int)model.CriticalStrike : token.CriticalStrike;
			token.AttackSpeed = (model.AttackSpeed != 0 && model.AttackSpeed != null) ? (int)model.AttackSpeed : token.AttackSpeed;
			token.Mastery = (model.Mastery != 0 && model.Mastery != null) ? (int)model.Mastery : token.Mastery;
			token.Versatility = (model.Versatility != 0 && model.Versatility != null) ? (int)model.Versatility : token.Versatility;
			token.Experience = (model.Experience != 0 && model.Experience != null) ? (int)model.Experience : token.Experience;
			token.Level = (model.Level != 0 && model.Level != null) ? (int)model.Level : token.Level;
			token.ImageUrl = (model.ImageUrl != string.Empty) ? model.ImageUrl : token.ImageUrl;
			token.IsBreeding = (model.IsBreeding) ? model.IsBreeding : token.IsBreeding;
			token.IsBrawling = (model.IsBreeding) ? model.IsBrawling : token.IsBrawling;

			db.Update(token);
			db.SaveChanges();

			return token;
		}
	}
}