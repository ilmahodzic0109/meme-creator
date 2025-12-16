import type { MemeConfig } from "../models/MemeConfig";

const BASE = import.meta.env.VITE_API_BASE_URL ?? "https://localhost:7012";

export async function createConfig(config: MemeConfig): Promise<number> {
  const res = await fetch(`${BASE}/api/config`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(config),
  });

  if (!res.ok) {
    const txt = await res.text().catch(() => "");
    throw new Error(txt || `HTTP ${res.status}`);
  }

  const data = await res.json();
  return data.id;
}

export async function updateConfigById(
  id: number,
  config: MemeConfig
): Promise<void> {
  const res = await fetch(`${BASE}/api/config/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(config),
  });

  if (!res.ok) {
    const txt = await res.text().catch(() => "");
    throw new Error(txt || `HTTP ${res.status}`);
  }
}
