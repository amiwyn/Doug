using System;
using System.Collections.Generic;

namespace Doug.Monsters
{
    public interface IMonsterFactory
    {
        Monster CreateMonster(string monsterId);
    }

    public class MonsterFactory : IMonsterFactory
    {
        private readonly Dictionary<string, Func<Monster>> _monsters;

        public MonsterFactory()
        {
            _monsters = new Dictionary<string, Func<Monster>> 
            {
                { AnimeTiddies.MonsterId, () => new AnimeTiddies() },
                { Kim.MonsterId, () => new Kim() },
                { APepe.MonsterId, () => new APepe() },
                { Beaubrun.MonsterId, () => new Beaubrun() },
                { Beauceron.MonsterId, () => new Beauceron() },
                { BlackTrudeau.MonsterId, () => new BlackTrudeau() },
                { DearPrimeMinister.MonsterId, () => new DearPrimeMinister() },
                { DesjardinsFraudster.MonsterId, () => new DesjardinsFraudster() },
                { Farmer.MonsterId, () => new Farmer() },
                { FerociousBear.MonsterId, () => new FerociousBear() },
                { ForeinStudent.MonsterId, () => new ForeinStudent() },
                { Francine.MonsterId, () => new Francine() },
                { Gabby.MonsterId, () => new Gabby() },
                { Hobo.MonsterId, () => new Hobo() },
                { Judith.MonsterId, () => new Judith() },
                { LePetitBum.MonsterId, () => new LePetitBum() },
                { Manon.MonsterId, () => new Manon() },
                { Mario.MonsterId, () => new Mario() },
                { Naruto.MonsterId, () => new Naruto() },
                { PiquerieManager.MonsterId, () => new PiquerieManager() },
                { Rat.MonsterId, () => new Rat() },
                { RogerRemblais.MonsterId, () => new RogerRemblais() },
                { Seagull.MonsterId, () => new Seagull() },
                { SexualPredator.MonsterId, () => new SexualPredator() },
                { SexyCatherine.MonsterId, () => new SexyCatherine() },
                { Sylvie.MonsterId, () => new Sylvie() },
                { ThirdBridge.MonsterId, () => new ThirdBridge() },
                { Undefined.MonsterId, () => new Undefined() },
                { Vapanielle.MonsterId, () => new Vapanielle() },
                { Waifu.MonsterId, () => new Waifu() },
                { Weeb.MonsterId, () => new Weeb() }
            };
        }

        public Monster CreateMonster(string monsterId)
        {
            return _monsters.GetValueOrDefault(monsterId)();
        }
    }
}
