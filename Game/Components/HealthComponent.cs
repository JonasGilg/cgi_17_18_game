using System;
using Engine;
using Engine.Component;
using Engine.Render;
using Game.GamePlay;

namespace Game.Components {
	public class HealthComponent : Component {
		private int currentHp;
		private readonly int maxHp;

		private readonly bool canBeInvulnerable;
		private const int INVULNERABILITY_TIME = 2000;
		private double invulnerableTill;
		private bool IsVulnerable => invulnerableTill < Time.TotalTime;

		private const double INVULNERABILITY_EFFECT_FREQUENZY = 100.0;

		public HealthComponent(GameObject gameObject, int maxHp = 100, bool canBeInvulnerable = false) : base(gameObject) {
			this.maxHp = maxHp;
			SetHealthpoints(maxHp);
			this.canBeInvulnerable = canBeInvulnerable;
		}

		private void SetHealthpoints(int val) => currentHp = val;
		public double GetCurrentHp() => currentHp;

		public void InstaKill() {
			if (IsVulnerable)
				currentHp = 0;
		}

		public void TakeDamage(int dmg) {
			if (IsVulnerable) {
				currentHp -= dmg;
				ResetInvulnerabilityTime();
			}
		}

		public void HealHp(int val) {
			if (currentHp + val > maxHp) {
				currentHp = maxHp;
			}
			else {
				currentHp += val;
			}
		}

		private double invulTimeStep;

		private void ResetInvulnerabilityTime() {
			if (canBeInvulnerable) {
				invulnerableTill = Time.TotalTime + INVULNERABILITY_TIME;
				invulTimeStep = Time.TotalTime + INVULNERABILITY_EFFECT_FREQUENZY;
			}
		}

		private bool Alive() => currentHp > 0;

		public override void Update() {
			InvulnerabilityEffect();
			AliveCheck();
		}

		private bool getsRendered = true;

		private void InvulnerabilityEffect() {
			if (canBeInvulnerable && !IsVulnerable) {
				if (Time.TotalTime > invulTimeStep) {
					if (getsRendered) {
						if (GameObject.SearchOptionalComponents(ComponentType.RENDER_COMPONENT, out var resultList)) {
							for (var i = 0; i < resultList.Count; i++) {
								RenderEngine.UnregisterDynamicRenderComponent((RenderComponent) resultList[i]);
							}
						}

						getsRendered = false;
					}
					else {
						if (GameObject.SearchOptionalComponents(ComponentType.RENDER_COMPONENT, out var resultList)) {
							for (var i = 0; i < resultList.Count; i++) {
								RenderEngine.RegisterDynamicRenderComponent((RenderComponent) resultList[i]);
							}
						}

						getsRendered = true;
					}

					invulTimeStep += INVULNERABILITY_EFFECT_FREQUENZY;
				}
			}
			else if (IsVulnerable && !getsRendered) {
				if (GameObject.SearchOptionalComponents(ComponentType.RENDER_COMPONENT, out var resultList)) {
					for (var i = 0; i < resultList.Count; i++) {
						RenderEngine.RegisterDynamicRenderComponent((RenderComponent) resultList[i]);
					}
				}

				getsRendered = true;
			}
		}

		private void AliveCheck() {
			if (!Alive()) {
				GamePlayEngine.RemoveObjectFromWorld(GameObject);
			}
		}

		public string HealthPointStatus() => $"{currentHp}/{maxHp}";
	}
}