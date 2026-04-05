import { test, expect } from "@playwright/test";

test.beforeEach(async ({ page }) => {
  await page.goto("/exercises");
});

test("Shows heading, links and button", async ({ page }) => {
  await expect(page.getByRole("heading", { name: "Exercise Library" })).toBeVisible();
  await expect(page.getByRole("link", { name: "Workouts" })).toBeVisible();
  await expect(page.getByRole("link", { name: "Exercises" })).toBeVisible();
  await expect(page.getByRole("button", { name: "Add" })).toBeVisible();
});

test("Opens Create Exercise modal when Add is clicked", async ({ page }) => {
  await page.getByRole("button", { name: "Add" }).click();
  await expect(page.getByRole("heading", { name: "Create Exercise" })).toBeVisible();
});

test("Modal has correct fields and buttons", async ({ page }) => {
  await page.getByRole("button", { name: "Add" }).click();
  await expect(page.getByText("Name")).toBeVisible();
  await expect(page.getByText("Description")).toBeVisible();
  await expect(page.getByRole("button", { name: "Cancel" })).toBeVisible();
  await expect(page.getByRole("button", { name: "Create" })).toBeVisible();
});

test("Cancel button closes the modal", async ({ page }) => {
  await page.getByRole("button", { name: "Add" }).click();
  await expect(page.getByRole("heading", { name: "Create Exercise" })).toBeVisible();
  await page.getByRole("button", { name: "Cancel" }).click();
  await expect(page.getByRole("heading", { name: "Create Exercise" })).not.toBeVisible();
});
