import { test, expect } from "@playwright/test";

test.beforeEach(async ({ page }) => {
  await page.goto("/workouts");
});

test("Shows heading and links", async ({ page }) => {
  await expect(page.getByRole("heading", { name: "Your Workouts" })).toBeVisible();
  await expect(page.getByRole("link", { name: "Workouts" })).toBeVisible();
  await expect(page.getByRole("link", { name: "Exercises" })).toBeVisible();
});
