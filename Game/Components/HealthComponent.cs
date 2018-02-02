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
		public bool CanBeInvulnerable;
		private int invulnerabilityTime = 2000; //milliseconds
		private double invulnerableTill;
		public bool IsVulnerable => invulnerableTill < Time.TotalTime;

		private readonly double INVULNERABILITY_EFFECT_FREQUENZY = 100.0;

		public HealthComponent(GameObject gameObject, int maxHP = 100, bool canBeInvulnerable=false) : base(gameObject) {
			this.maxHP = maxHP;
			setHealthpoints(maxHP);
			CanBeInvulnerable = canBeInvulnerable;
		}

		//static hp setter/getter
		public void setHealthpoints(int val) {
			currentHP = val;
		}

		public double getCurrentHP() => currentHP;

		
		//interactive dmg taking
		public void instaKill() {
			if(IsVulnerable)
			currentHP = 0;
		}
		
		public void takeDamage(int dmg) {
			if (IsVulnerable) {
				currentHP -= dmg;
				resetInvulnerabilityTime();
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

		private double invulTimeStep = 0;
		private void resetInvulnerabilityTime() {
			if (CanBeInvulnerable) {
				invulnerableTill = Time.TotalTime + invulnerabilityTime;
				invulTimeStep = Time.TotalTime + INVULNERABILITY_EFFECT_FREQUENZY ;
			}
			
		}

		public Boolean alive() => currentHP > 0;
		
		
		public override void Update() {
			invulnerabilityEffect();
			aliveCheck();
			
		}

		private bool getsRendered = true;
		private void invulnerabilityEffect() {
			if (CanBeInvulnerable && !IsVulnerable) {
				if (Time.TotalTime > invulTimeStep) {
					if (getsRendered) {
						if (GameObject.searchOptionalComponents(ComponentType.RENDER_COMPONENT, out var resultList)) {
							for (int i = 0; i < resultList.Count; i++) {
								RenderEngine.UnregisterStaticRenderComponent((RenderComponent)resultList[i]);
							}
						}
						getsRendered = false;
					}
					else {
						if (GameObject.searchOptionalComponents(ComponentType.RENDER_COMPONENT, out var resultList)) {
							for (int i = 0; i < resultList.Count; i++) {
								RenderEngine.RegisterStaticRenderComponent((RenderComponent)resultList[i]);
							}
						}
						getsRendered = true;
					}
					invulTimeStep += INVULNERABILITY_EFFECT_FREQUENZY;
				}
			}
			else if (IsVulnerable && !getsRendered) {
				if (GameObject.searchOptionalComponents(ComponentType.RENDER_COMPONENT, out var resultList)) {
					for (int i = 0; i < resultList.Count; i++) {
						RenderEngine.RegisterStaticRenderComponent((RenderComponent)resultList[i]);
					}
				}
				getsRendered = true;
			}
		}

		private void aliveCheck() {
			if (!alive()) {
				GamePlayEngine.RemoveObjectFromWorld(GameObject);
			}
		}

		public String healthPointStatus() {
			return $"{currentHP}/{maxHP}";
		}
	}
}