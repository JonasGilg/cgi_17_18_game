using System;
using Engine;
using Engine.Component;
using Engine.Render;
using Game.GamePlay;

namespace Game.Components {
	public class HealthComponent : Component {
		
		//stats
		private int currentHP;
		private int maxHP;

		
		//invulnerabilitystats
		public bool Invulnerability = false;
		private int invulnerabilityTime = 2000; //milliseconds
		private double invulnerableTill;
		public bool IsInvulnerable => invulnerableTill > Time.TotalTime;

		public HealthComponent(GameObject gameObject, int maxHP = 100, bool invulnerability=true) : base(gameObject) {
			this.maxHP = maxHP;
			setHealthpoints(maxHP);
			Invulnerability = invulnerability;
		}

		//static hp setter/getter
		public void setHealthpoints(int val) {
			currentHP = val;
		}

		public double getCurrentHP() => currentHP;

		
		//interactive dmg taking
		
		
		public void takeDamage(int dmg) {
			if (!IsInvulnerable) {
				currentHP -= dmg;
				if (!alive()) {
					GamePlayEngine.GameOver();
				} else {
					refreshInvulnerability();
				}
			}
		}

		public void healHP(int val) {
			if (currentHP + val > maxHP) {
				currentHP = maxHP;
			}
			else {
				currentHP += val;
			}
		}

		private void refreshInvulnerability() {
			invulnerableTill = Time.TotalTime + invulnerabilityTime;
		}

		public Boolean alive() => currentHP > 0;
		
		
		public override void Update() {
			/*if (!alive()) {
				GamePlayEngine.GameOver();
			}*/
		}

		public String healthPointStatus() {
			return $"HP: {currentHP}/{maxHP}";
		}
	}
}