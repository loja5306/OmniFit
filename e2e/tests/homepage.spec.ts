import { test, expect } from "@playwright/test";

test("Has title", async ({ page }) => {
  await page.goto("/");

  await expect(page).toHaveTitle("OmniFit");
});

test("Shows welcome message and navigation bar", async ({ page }) => {
  await page.goto("/");

  await expect(page.getByRole("heading", { name: "Welcome back!" }))
    .toBeVisible();
  await expect(page.getByRole("link", { name: "Workouts" }))
    .toBeVisible();
  await expect(page.getByRole("link", { name: "Exercises" }))
    .toBeVisible();
});
